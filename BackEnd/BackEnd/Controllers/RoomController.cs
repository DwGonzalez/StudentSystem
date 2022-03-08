using BackEnd.Data;
using BackEnd.Data.Entities;
using BackEnd.Enums;
using BackEnd.Models;
using BackEnd.Models.BindingModel;
using BackEnd.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly AppDBContext _dbContext;

        public RoomController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost("AddRoom")]
        public async Task<object> AddRoom([FromBody] AddRoomBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var roomExists = _dbContext.Rooms.Where(x => x.RoomName == model.RoomName).ToList();
                    if (roomExists.Count > 0)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Room already exists", null));
                    }

                    var room = new Room()
                    {
                        RoomName = model.RoomName
                    };
                    _dbContext.Rooms.Add(room);
                    _dbContext.SaveChanges();

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Room has been registered", null));

                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Parameters missing", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllRooms")]
        public async Task<object> GetAllRooms()
        {
            try
            {
                var rooms = _dbContext.Rooms.Select(x => new RoomDTO(x.RoomName, x.Id));

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", rooms));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
