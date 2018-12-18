using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotebookAppApi.Infrastrcuture;
using NotebookAppApi.Interfaces;
using NotebookAppApi.Model;

namespace NotebookAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NotesController (INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<Note>> Get()
        {
            return await _noteRepository.GetAllNotes();
        }

        // Get api/notes/5
        [HttpGet("{id}")]
        public async Task<Note> Get(string id)
        {
            return await _noteRepository.GetNote(id) ?? new Note();
        }


    }
}