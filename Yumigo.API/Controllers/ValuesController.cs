using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Yumigo.API.DbContext;
using Yumigo.API.Migrations;
using Yumigo.API.Models;
using Yumigo.API.Models.DTO;
using Yumigo.API.Utility;

namespace Yumigo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;

        public ValuesController(ApplicationDbContext db, ApiResponse response)
        {
            _db = db;
            _response = response;
        }

        [HttpGet]
        public ActionResult<ApiResponse> GetOrders(string userId="") 
        {
            IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeaders.Include(_ => _.OrderDetails)
                .ThenInclude(_ => _.MenuItem).OrderByDescending(_ => _.OrderDetails);

            if (!string.IsNullOrEmpty(userId)) 
            {
                orderHeaderList = orderHeaderList.Where(_=>_.ApplicationUserId== userId);
            }

            _response.Result = orderHeaderList;
            _response.StatusCode= HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{orderId:int}")]
        public ActionResult<ApiResponse> GetOrder(int orderId)
        {

            if (orderId < 0) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Invalid orderId");
                return BadRequest(_response);
            }
            OrderHeader? orderHeader = _db.OrderHeaders.Include(_ => _.OrderDetails)
                .ThenInclude(_ => _.MenuItem).FirstOrDefault(_ => _.OrderHeaderId== orderId);

            if (orderHeader == null) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Order not dounf");
                return BadRequest(_response);
            }     

            _response.Result = orderHeader;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public ActionResult<ApiResponse> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OrderHeader orderHeader = new()
                    {
                        PickUpName = orderHeaderDTO.PickUpName,
                        PickUpPhoneNumber = orderHeaderDTO.PickUpPhoneNumber,
                        PickUpEmail = orderHeaderDTO.PickUpEmail,
                        OrderDate = DateTime.Now,
                        OrderTotal = orderHeaderDTO.OrderTotal,
                        Status = SD.status_confirmed,
                        TotalItem = orderHeaderDTO.TotalItem,
                        ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                    };
                    _db.OrderHeaders.Add(orderHeader);
                    _db.SaveChanges();

                    foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderHeaderId = orderHeader.OrderHeaderId,
                            MenuItemId = orderDetailDTO.MenuItemId,
                            Quantity = orderDetailDTO.Quantity,
                            ItemName = orderDetailDTO.ItemName,
                            Price = orderDetailDTO.Price,
                        };
                        _db.OrderDetails.Add(orderDetail);     
                    }
                    _db.SaveChanges();
                    _response.Result = orderHeader;
                    orderHeader.OrderDetails = [];
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtAction(nameof(GetOrder), new {orderId= orderHeader.OrderHeaderId}, _response);

                }
                else
                {
                    _response.IsSuccess=false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = ModelState.Values.SelectMany(_ => _.Errors).
                        Select(_ => _.ErrorMessage).ToList();
                    return BadRequest(_response);

                }
            }
            catch (Exception ex) 
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }


    }
}
