﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index(RequestGetList dto)
        {
            ResultPagination<Product> datafinal = GetData(dto);

            return View(datafinal);
        }

        private ResultPagination<Product> GetData(RequestGetList dto)
        {
            var data = _context.Product.OrderByDescending(p => p.Id).AsQueryable();
            var TotalCount = data.Count();

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                data = data.Where(p => p.Description.Contains(dto.SearchKey)
                                        || p.Name.Contains(dto.SearchKey)
                                        || p.Price.Equals(dto.SearchKey)

                                        || p.Id.ToString().Equals(dto.SearchKey)).OrderByDescending(p => p.Id).AsQueryable();

            }
            var dataList = data.ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();
            var pagesize = dto.PageSize ?? 10;
            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<Product>
            {
                CurrentPage = dto.Page ?? 1,
                NumberOfPage = NumberOfPage,
                PageSize = dto.PageSize ?? 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Product dto, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (dto.Id == 0)
                {
                    //add
                    await _context.Product.AddAsync(dto);
                    foreach (var file in files)
                    {
                        var resultUpload = new UploadFile(_environment).UploadFileFunction(file, @"Product\");

                        await _context.ProductImage.AddAsync(new ProductImage
                        {
                            ProductId = dto.Id,
                            Name = resultUpload.FileName,
                            Src = resultUpload.FileNameAddress
                        });
                        await _context.SaveChangesAsync();

                    }
                }
                else
                {
                    //update
                    var prdata = await _context.Category.FindAsync(dto.Id);
                    dto.InserDate = prdata.InserDate;

                    _context.Product.Update(dto);
                    foreach (var file in files)
                    {
                        var resultUpload = new UploadFile(_environment).UploadFileFunction(file, @"Product\");

                        await _context.ProductImage.AddAsync(new ProductImage
                        {
                            ProductId = dto.Id,
                            Name = resultUpload.FileName,
                            Src = resultUpload.FileNameAddress
                        });
                        await _context.SaveChangesAsync();

                    }
                }
                await _context.SaveChangesAsync();
            }

            var Key = ModelState.FirstOrDefault(p => p.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid).Key;


            ModelState.AddModelError("CustomError", $"Error : " + $"{Key} is required");
            RequestGetList resultdto = new();

            ResultPagination<Product> datafinal = GetData(resultdto);

            return View(datafinal);


        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.Product.Remove(await _context.Product.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
