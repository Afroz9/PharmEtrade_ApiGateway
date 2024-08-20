﻿using BAL.Models;
using BAL.ResponseModels;
using BAL.ViewModel;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepo _productRepo;

        public ProductController(IProductsRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var response = await _productRepo.AddProduct(product);
            return Ok(response);
        }

        [HttpPost]
        [Route("Image/Upload")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            UploadResponse response = await _productRepo.UploadImage(image);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productRepo.GetAllProducts();
            return Ok(response);
        }

        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct(ProductFilter productviewmodel)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productviewmodel.ImageUrl.CopyToAsync(memoryStream);
                    var result = await _productRepo.InsertAddProduct(productviewmodel, memoryStream, productviewmodel.ImageUrl.FileName);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Author: [swathi]
        // Created Date: [10/07/2024]
        // Description: Method for BulkInsertProducts
        [HttpPost("InsertOrUploadProduct")]
        public async Task<IActionResult> InsertOrUploadProduct(IFormFile file)
        {
            try
            {
                var result = await _productRepo.ProcessExcelFileAsync(file);
                return Ok(new { status = 200, message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Author: [Mamatha]
        // Created Date: [04/07/2024]
        // Description: Method for EditProductDetails
        [HttpPost("EditProductDetails")]
        public async Task<IActionResult> EditProductDetails(int AddproductID, ProductFilter productviewmodel, IFormFile ImageUrl)
        {
            if (productviewmodel == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    if (ImageUrl != null)
                    {
                        await ImageUrl.CopyToAsync(memoryStream);
                    }

                    var result = await _productRepo.EditProductDetails(AddproductID, productviewmodel, ImageUrl != null ? memoryStream : Stream.Null, ImageUrl?.FileName);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Author: [swathi]
        // Created Date: [02/07/2024]
        // Description: Method for AddtoCartProducts
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel addToCartModel)
        {
            try
            {
                var userId = addToCartModel.Userid;
                var imageId = addToCartModel.Imageid;
                var productId = addToCartModel.ProductId;

                var result = await _productRepo.InsertAddToCartProduct(addToCartModel);

                return Ok(new { AddToCartId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpGet("GetCartByCustomerID/{CustomerID}")]
        public async Task<ActionResult<List<UserProductViewModel>>> GetCartByCustomerID(string CustomerID)
        {
            var products = await _productRepo.GetCartByCustomerID(CustomerID);
            return Ok(products);
        }

        // Author: [swathi]
        // Created Date: [04/07/2024]
        // Description: Method for  Delete CartProduct
        [HttpPost("SoftDeleteAddtoCartProduct")]
        public async Task<IActionResult> SoftDeleteAddtoCartProduct([FromBody] int addToCartId)
        {
            try
            {
                var result = await _productRepo.SoftDeleteAddtoCartProduct(addToCartId);
                if (result.status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, result.message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  Insert WishlistProduct
        [HttpPost("AddWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] Wishlistviewmodel wishlistviewmodel)
        {
            try
            {
                var userId = wishlistviewmodel.Userid;
                var imageId = wishlistviewmodel.Imageid;
                var productId = wishlistviewmodel.ProductId;

                var result = await _productRepo.InsertWishlistproduct(wishlistviewmodel);

                return Ok(new { Wishlistid = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  GetwishlistProduct by userid
        [HttpGet("GetwishlistByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<UserProductViewModel>>> GetwishlistByUserId(int userId)
        {
            var products = await _productRepo.GetwhislistByUserId(userId);
            return Ok(products);
        }

        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  Delete WishListProduct
        [HttpPost("DeleteWishlistProduct")]
        public async Task<IActionResult> DeleteWishlistProduct([FromBody] int wishlistid)
        {
            try
            {
                var result = await _productRepo.DeleteWishlistproduct(wishlistid);
                if (result.status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, result.message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}