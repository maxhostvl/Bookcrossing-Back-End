﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Implementation
{
    public class RequestService : IRequestService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        public RequestService(IRepository<Request> requestRepository,IRepository<Book> bookRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        /// <inheritdoc />
        public async Task<RequestDto> Make(int userId, int bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);
            var request = new Request()
            {
                BookId = book.Id,
                OwnerId = book.UserId,
                UserId = userId,
                RequestDate = DateTime.UtcNow
            };
            _requestRepository.Add(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public IEnumerable<RequestDto> Get(int bookId)
        {
            return _mapper.Map<IEnumerable<RequestDto>>(_requestRepository.GetAll()
                .Include(i => i.Book).ThenInclude(i=>i.BookAuthor).ThenInclude(i=>i.Author)
                .Include(i => i.Book).ThenInclude(i=>i.BookGenre).ThenInclude(i=>i.Genre)
                .Include(i => i.Owner).ThenInclude(i=>i.UserLocation).ThenInclude(i => i.Location)
                .Include(i => i.User).ThenInclude(i=>i.UserLocation).ThenInclude(i=>i.Location)
                .Where(i => i.BookId == bookId));
        }
        /// <inheritdoc />
        public async Task<RequestDto> Approve(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            request.ReceiveDate = DateTime.UtcNow;
            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
        /// <inheritdoc />
        public async Task<RequestDto> Remove(int requestId)
        {
            var request = await _requestRepository.FindByIdAsync(requestId);
            if (request == null)
                return null;
            _requestRepository.Remove(request);
            await _requestRepository.SaveChangesAsync();
            return _mapper.Map<RequestDto>(request);
        }
    }
}