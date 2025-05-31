using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Yumigo.API.DbContext;
using Yumigo.API.Models;
using Yumigo.API.Models.DTO;

namespace Yumigo.API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApiResponse _response;
        private readonly IWebHostEnvironment _env;
        public MenuItemController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _response = new ApiResponse();
            _env = env;
        }

        [HttpGet]
        public IActionResult GetMenuItems()
        {
            List<MenuItem> menuItems = _context.menuItems.ToList();
            List<OrderDetail> orderDetailsWithRating = _context.OrderDetails.Where(_=>_.Rating != null).ToList();

            foreach ( var menuItem in menuItems)
            {
                var rating = orderDetailsWithRating.Where(_ => _.MenuItemId == menuItem.Id).Select(_ => _.Rating.Value);
                double avgRating = rating.Any() ? rating.Average() : 0;
                menuItem.Rating = avgRating;
            }

            _response.Result = menuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name="GetMenuItem")]
        public IActionResult GetMenuItem(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            else
            {
                MenuItem? menuItem = _context.menuItems.FirstOrDefault(_ => _.Id == id);
                List<OrderDetail> orderDetailsWithRating = _context.OrderDetails.Where(_ => _.Rating != null&& _.MenuItemId==menuItem.Id).ToList();

           
                    var rating = orderDetailsWithRating.Select(_ => _.Rating.Value);
                    double avgRating = rating.Any() ? rating.Average() : 0;
                    menuItem.Rating = avgRating;
           


                _response.Result = menuItem;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages = ["File is Required"];
                        return BadRequest(_response);
                    }
                    var imagesPath = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(imagesPath)) 
                    {
                        Directory.CreateDirectory(imagesPath);
                    }
                    var filePath = Path.Combine(imagesPath, menuItemCreateDTO.File.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    //Uploading the image
                    using(var stream= new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(stream);
                    }

                    MenuItem menuItem = new()
                    {
                        Name = menuItemCreateDTO.Name,
                        Description = menuItemCreateDTO.Description,
                        Price = menuItemCreateDTO.Price,
                        Category = menuItemCreateDTO.Category,
                        SpecialTag = menuItemCreateDTO.SpecialTag,
                        Image = "images/" + menuItemCreateDTO.File.FileName,
                    };
                    _context.menuItems.Add(menuItem);
                    await _context.SaveChangesAsync();

                    _response.Result = menuItemCreateDTO;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new {id= menuItem.Id}, _response);

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(ex);
            }
            return BadRequest(_response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id,[FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDTO.File == null || menuItemUpdateDTO.Id!=id)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                    MenuItem menuItemFromDb = await _context.menuItems.FirstOrDefaultAsync(_ => _.Id == id);

                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }


                    menuItemFromDb.Name = menuItemFromDb.Name;
                    menuItemFromDb.Description = menuItemFromDb.Description;
                    menuItemFromDb.Price=menuItemFromDb.Price;
                    menuItemFromDb.Category = menuItemFromDb.Category;
                    menuItemFromDb.SpecialTag = menuItemFromDb.SpecialTag;

                    if (menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length >0) 
                    {
                        var imagesPath = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(imagesPath))
                        {
                            Directory.CreateDirectory(imagesPath);
                        }
                        var filePath = Path.Combine(imagesPath, menuItemUpdateDTO.File.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFromDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await menuItemUpdateDTO.File.CopyToAsync(stream);
                        }

                        menuItemFromDb.Image = "images/" + menuItemUpdateDTO.File.FileName;

                        
                    }

                    _context.menuItems.Update(menuItemFromDb);
                    await _context.SaveChangesAsync();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);


                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(ex);
            }
            return BadRequest(_response);
        }



        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id==0)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                    MenuItem? menuItemFromDb = await _context.menuItems.FirstOrDefaultAsync(_ => _.Id == id);

                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                    var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFromDb.Image);
                    if (System.IO.File.Exists(filePath_OldFile))
                    {
                        System.IO.File.Delete(filePath_OldFile);
                    }

                    _context.menuItems.Remove(menuItemFromDb);
                    await _context.SaveChangesAsync();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);


                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(ex);
            }
            return BadRequest(_response);
        }
    }
}
