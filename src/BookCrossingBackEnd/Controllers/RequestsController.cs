﻿using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using BookCrossingBackEnd.Filters;
using Microsoft.AspNetCore.Authorization;

namespace BookCrossingBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        //[Authorize]
        [Route("{bookId}")]
        [HttpPost]
        public async Task<ActionResult<RequestDto>> Make([FromRoute] int bookId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase))?.Value);
            return await _requestService.Make(userId, bookId);
        }
        //[Authorize]
        [Route("{bookId}")]
        [HttpGet]
        public async Task<ActionResult<PaginationDto<RequestDto>>> Get([FromRoute] int bookId, [FromQuery] QueryParameters query)
        {
            return Ok(await _requestService.Get(bookId, query));
        }
        //[Authorize]
        [ModelValidationFilter]
        [Route("{requestId}")]
        [HttpPut]
        public async Task<ActionResult<RequestDto>> Approve([FromRoute] int requestId)
        {
            var updated = await _requestService.Approve(requestId);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }
        //[Authorize]
        [ModelValidationFilter]
        [Route("{requestId}")]
        [HttpDelete]
        public async Task<ActionResult<RequestDto>> Remove([FromRoute] int requestId)
        {
            var removed = await _requestService.Remove(requestId);
            if (!removed)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
