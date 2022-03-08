namespace BackEnd.Models.DTO
{
    public class RoomDTO
    {
        public RoomDTO(string roomName, int roomId)
        {
            RoomName = roomName;
            RoomId = roomId;
        }
        public string RoomName { get; set; }
        public int RoomId { get; set; }
    }
}
