﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Controllers
{
    public class NewsController : Controller
    {
        private readonly DataBaseContext _context;

        public NewsController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(RequestGetList dto)
        {
            var data = _context.News.OrderByDescending(p => p.Id).AsQueryable();
            var TotalCount = data.Count();

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                data = data.Where(p => p.Text.Contains(dto.SearchKey)
                                        || p.Caption.Contains(dto.SearchKey)

                                        || p.Id.ToString().Equals(dto.SearchKey)).OrderByDescending(p => p.Id);

            }
            var dataList = data.ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();

            var pagesize = dto.PageSize ?? 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<News>
            {
                CurrentPage = dto.Page ?? 1,
                NumberOfPage = NumberOfPage,
                PageSize = dto.PageSize ?? 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };

            var finalData = new HomeData
            {
                About = _context.About.FirstOrDefault(),
                News = datafinal
            };

            return View(finalData);
        }

        public IActionResult Details(RequestGetList dto)
        {

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                if (_context.News.FirstOrDefault(p => p.Id == Convert.ToInt64(dto.SearchKey)) == null)
                {
                    return RedirectToAction("Index");
                }
                var finalData = new HomeData
                {
                    About = _context.About.FirstOrDefault(),
                    NewsDetail = _context.News.FirstOrDefault(p => p.Id == Convert.ToInt64(dto.SearchKey))
                };

                return View(finalData);
            }

            return RedirectToAction("Index");
        }
    }
}
