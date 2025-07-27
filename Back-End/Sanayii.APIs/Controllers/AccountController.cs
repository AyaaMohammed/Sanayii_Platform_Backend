using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sanayii.APIs.Services;
using Sanayii.Core.DTOs.AccountDTOs;
using Sanayii.Core.Entities;
using Sanayii.Services;
using Sanayii.Repository.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using UAParser;
using Microsoft.EntityFrameworkCore;

namespace Sanayii.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly EmailSenderService emailSender;
        private readonly SMSSenderService smsSender;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SanayiiContext db;
        IMapper mapper;
        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            SMSSenderService smsSender,
            EmailSenderService emailSender,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper, SanayiiContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.smsSender = smsSender;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.db = db;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            // Find user by username or email
            AppUser user = await userManager.FindByNameAsync(model.Username) ?? await userManager.FindByEmailAsync(model.Username);
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            // Check if email is confirmed (if required)
            if (!user.EmailConfirmed)
            {
                return BadRequest("Email is not confirmed. Please check your inbox.");
            }

            // Check password
            var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return BadRequest("User account is locked out. Please try again later.");
                if (result.IsNotAllowed)
                    return BadRequest("Login not allowed for this user.");
                return BadRequest("Invalid username or password");
            }

            // Create JWT Token
            var userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("L6scvGt8D3yU5vAqZt9PfMxW2jNkRgT7!@#$%"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:7234/",
                audience: "http://localhost:4200/",
                expires: DateTime.UtcNow.AddHours(1),
                claims: userClaims,
                signingCredentials: creds
            );

            return Ok(new
            {
                message = "Login successful",
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer user = mapper.Map<Customer>(model);
            var res = await userManager.CreateAsync(user, model.Password);
            if (res.Succeeded)
            {
                var resRole = await userManager.AddToRoleAsync(user, "Customer");
                if (resRole.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    if (model.PhoneNumbers != null && model.PhoneNumbers.Count > 0)
                    {
                        foreach (var phone in model.PhoneNumbers)
                        {
                            db.Add(new UserPhones
                            {
                                UserId = user.Id,
                                PhoneNumber = phone
                            });
                        }

                        await db.SaveChangesAsync();
                    }
                    await SendConfirmationEmail(user);
                    // Generate JWT Token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("L6scvGt8D3yU5vAqZt9PfMxW2jNkRgT7!@#$%"); // Use a secure key

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Ok(new { token = tokenHandler.WriteToken(token) });
                }
                else
                {
                    return BadRequest("Failed to assign role");
                }
            }
            else
            {
                return BadRequest(res.Errors.Select(e => e.Description));
            }
        }
        [AllowAnonymous]
        [HttpGet("ExternalLogin")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl }, Request.Scheme);
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [HttpGet("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            // إضافة تعطيل Claim Mapping
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                var errorUrl = "http://localhost:4200/login?error=EmailMismatch";
                return Redirect(errorUrl);
            }

            var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    var errorUrl = "http://localhost:4200/login?error=EmailMismatch";
                    return Redirect(errorUrl);
                }
            }
            else
            {
                var emailFromJwt = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (user.Email != emailFromJwt)
                {
                    var errorUrl = "http://localhost:4200/login?error=EmailMismatch";
                    return Redirect(errorUrl);
                }
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("L6scvGt8D3yU5vAqZt9PfMxW2jNkRgT7!@#$%");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var frontendUrl = $"http://localhost:4200/login?token={jwtToken}";

            if (!string.IsNullOrEmpty(returnUrl))
            {
                frontendUrl = $"{frontendUrl}&returnUrl={Uri.EscapeDataString(returnUrl)}";
            }

            return Redirect(frontendUrl);
        }

        private async Task SendConfirmationEmail(AppUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Proper Encoding (Convert to Base64)
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            var encodedToken = WebUtility.UrlEncode(Convert.ToBase64String(tokenBytes));

            // بدل ما تبني اللينك على Request.Host (الـ API)، حط لينك الواجهة (Angular)
            var angularClientUrl = "http://localhost:4200"; // ✨ أو تحط URL بتاع Angular بتاعك لو مستضيفه
            var confirmationLink = $"{angularClientUrl}/confirm-email-register?userId={user.Id}&token={encodedToken}";

            var subject = "Confirm Your Email - Sanayii";
            var messageBody = $@"
            <div style=""font-family:Arial,Helvetica,sans-serif;font-size:16px;line-height:1.6;color:#333;"">
                <p>Hi {user.FName} {user.LName},</p>
                <p>Thank you for registering with <strong>Sanayii</strong>. Please confirm your email by clicking the button below:</p>
                <p>
                    <a href=""{HtmlEncoder.Default.Encode(confirmationLink)}"" 
                       style=""background-color:#007bff;color:#fff;padding:10px 20px;text-decoration:none;
                              font-weight:bold;border-radius:5px;display:inline-block;"">
                        Confirm Email
                    </a>
                </p>
                <p>If the button doesn’t work, copy and paste this URL into your browser:</p>
                <p><a href=""{HtmlEncoder.Default.Encode(confirmationLink)}"" style=""color:#007bff;text-decoration:none;"">{HtmlEncoder.Default.Encode(confirmationLink)}</a></p>
                <p>If you did not sign up for this account, please ignore this email.</p>
                <p>Best Regards,<br />The Sanayii Team</p>
            </div>
        ";

            await emailSender.SendEmailAsync(user.Email, subject, messageBody, true);
        }


        [HttpPost("ResendConfirmationEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { message = "Please provide a valid email address." });

           
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound(new { message = "User not found." });
            var res = await userManager.IsEmailConfirmedAsync(user);
            if (res)
            {
                return Ok(new { message = "This Email is Already confirmed!" });
            }

            try
            {
                await SendConfirmationEmail(user);
                return Ok(new { message = "A new confirmation email has been sent." });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while sending the confirmation email. Please try again later." });
            }
        }
        private async Task SendForgotPasswordEmail(string email, AppUser user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var angularAppUrl = "http://localhost:4200/reset-password";
            var passwordResetLink = $"{angularAppUrl}?email={WebUtility.UrlEncode(email)}&token={encodedToken}";


            var safeLink = HtmlEncoder.Default.Encode(passwordResetLink);

            var subject = "Reset Your Password";

            var messageBody = $@"
                <div style=""font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #333; line-height: 1.5; padding: 20px;"">
                    <h2 style=""color: #007bff; text-align: center;"">Password Reset Request</h2>
                    <p>Hi {user.FName} {user.LName},</p>

                    <p>We received a request to reset your password for your <strong>Sanayii</strong> account. Click the button below to reset your password:</p>

                    <div style=""text-align: center; margin: 20px 0;"">
                        <a href=""{safeLink}"" 
                           style=""background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; font-weight: bold; border-radius: 5px; display: inline-block;"">
                            Reset Password
                        </a>
                    </div>

                    <p>If the button above doesn’t work, copy and paste this URL into your browser:</p>
                    <p style=""background-color: #f8f9fa; padding: 10px; border: 1px solid #ddd; border-radius: 5px;"">
                        <a href=""{safeLink}"" style=""color: #007bff; text-decoration: none;"">{safeLink}</a>
                    </p>

                    <p>If you did not request this reset, please ignore this email.</p>

                    <p>Thank you,<br />The Sanayii Team</p>
                </div>";

            await emailSender.SendEmailAsync(email, subject, messageBody, IsBodyHtml: true);
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                await SendForgotPasswordEmail(user.Email, user);
                return Ok(new { message = "Email sent successfully." });
            }
            return NotFound(new { message = "User not found." });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
                return NotFound(new { message = "User not found." });


            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Password reset successfully." });
            }

            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { message = "رابط التأكيد غير صالح." });
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." });
            }

            try
            {
                // فك تشفير التوكن
                var decodedTokenBytes = Convert.FromBase64String(WebUtility.UrlDecode(token));
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await userManager.ConfirmEmailAsync(user, decodedToken);
                if (result.Succeeded)
                {
                    // إرجاع رسالة النجاح مع رابط لتوجيه المستخدم إلى صفحة تسجيل الدخول
                    return Ok(new
                    {
                        message = "تم تأكيد البريد الإلكتروني بنجاح ✅",
                        redirectToLogin = true // إضافة خاصية لتوجيه المستخدم إلى صفحة تسجيل الدخول
                    });
                }
                else
                {
                    return BadRequest(new { message = "فشل في تأكيد البريد الإلكتروني." });
                }
            }
            catch (FormatException)
            {
                return BadRequest(new { message = "رمز التأكيد غير صالح." });
            }
        }

       
        private async Task SendPasswordChangedNotificationEmail(string email, AppUser user, string device)
        {
            var subject = "Your Password Has Been Changed";

            var messageBody = $@"
        <div style=""font-family: Arial, Helvetica, sans-serif; font-size: 16px; color: #333; line-height: 1.5; padding: 20px;"">
            <h2 style=""color: #007bff; text-align: center;"">Password Change Notification</h2>
            <p style=""margin-bottom: 20px;"">Hi {user.FName} {user.LName},</p>

            <p>We wanted to let you know that your password for your <strong>Sanayii</strong> account was successfully changed.</p>

            <div style=""margin: 20px 0; padding: 10px; background-color: #f8f9fa; border: 1px solid #ddd; border-radius: 5px;"">
                <p><strong>Date and Time:</strong> {DateTime.UtcNow:dddd, MMMM dd, yyyy HH:mm} UTC</p>
                <p><strong>Device:</strong> {device}</p>
            </div>

            <p>If you did not make this change, please reset your password immediately or contact support for assistance:</p>

            <p style=""margin-top: 30px;"">Thank you,<br />The Sanayii Team</p>
        </div>
        ";

            await emailSender.SendEmailAsync(email, subject, messageBody, IsBodyHtml: true);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data submitted.", errors = ModelState });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { message = "User not found. Please log in again." });
            }

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var device = GetDeviceInfo(HttpContext);

                await SendPasswordChangedNotificationEmail(user.Email, user, device);

                await signInManager.RefreshSignInAsync(user);

                return Ok(new { message = "Password changed successfully. A confirmation email has been sent." });
            }
            else
            {
                return BadRequest(new { message = "An error occurred while changing your password. Please try again." });
            }
        }
        private string GetDeviceInfo(HttpContext httpContext)
        {
            try
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (string.IsNullOrEmpty(userAgent))
                {
                    return "Unknown Device";
                }

                var parser = Parser.GetDefault();
                var clientInfo = parser.Parse(userAgent);

                var os = clientInfo.OS.ToString();
                var browser = clientInfo.UA.ToString();

                return $"{browser} on {os}";
            }
            catch (Exception ex)
            {
                return "Unknown Device";
            }
        }
    }
}