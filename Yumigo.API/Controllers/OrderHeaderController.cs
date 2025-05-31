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
    public class OrderHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;

        public OrderHeaderController(ApplicationDbContext db, ApiResponse response)
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



        [HttpPut("{orderId:int}")]
        public ActionResult<ApiResponse> UpdateOrder(int orderId,[FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (orderId!= orderHeaderUpdateDTO.OrderHeaderId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Invalid Id");
                        return BadRequest(_response);
                    }



                    OrderHeader? orderHeaderFromDb = _db.OrderHeaders
                        .FirstOrDefault(_=>_.OrderHeaderId == orderId);

                    if (orderHeaderFromDb == null)
                    {
                        _response.IsSuccess=!false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Order not found");
                        return NotFound(_response);
                    }

                    if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickUpName)) 
                    {
                        orderHeaderFromDb.PickUpName = orderHeaderUpdateDTO.PickUpName;
                    }
                    if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickUpPhoneNumber))
                    {
                        orderHeaderFromDb.PickUpPhoneNumber = orderHeaderUpdateDTO.PickUpPhoneNumber;
                    }
                    if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickUpEmail)) 
                    {
                        orderHeaderFromDb.PickUpEmail = orderHeaderUpdateDTO.PickUpEmail;
                    }
                    if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
                    {
                        if (orderHeaderFromDb.Status.Equals(SD.status_confirmed, StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderUpdateDTO.Status.Equals(SD.status_readyForPickup, StringComparison.InvariantCultureIgnoreCase)
                            )
                        {
                            orderHeaderFromDb.Status = SD.status_readyForPickup;
                        }

                        if (orderHeaderFromDb.Status.Equals(SD.status_confirmed, StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderUpdateDTO.Status.Equals(SD.status_Completed, StringComparison.InvariantCultureIgnoreCase)
                            )
                        {
                            orderHeaderFromDb.Status = SD.status_Completed;
                        }

                        if (orderHeaderFromDb.Status.Equals(SD.status_Cancelled, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_Cancelled;
                        }
                    }

                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                   
                }
                else
                {
                    _response.IsSuccess = false;
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
