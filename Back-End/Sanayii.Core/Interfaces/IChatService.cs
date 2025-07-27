using Sanayii.Core.DTOs.ChatDTOs;
using System.Threading.Tasks;

namespace Sanayii.Core.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDTO> SendMessageAsync(ChatRequestDTO request);
    }
}