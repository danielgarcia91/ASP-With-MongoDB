﻿using System;
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

        //GET api/notes/text/date/size
        // example: http://localhost:53617/api/notes/Test/2018-01-01/10000
        [NoCache]
        [HttpGet(template: "{bodyText}/{updateFrom}/{headerSizeLimit}")]
        public async Task<IEnumerable<Note>> Get (string bodyText,
                                                  DateTime updatedFrom,
                                                  long headerSizeLimit)
        {
            return await _noteRepository.GetNote(bodyText, updatedFrom, headerSizeLimit) ?? new List<Note>();
        }

        //POST api/notes
        [HttpPost]
        public void Post([FromBody] NoteParam newNote)
        {
            _noteRepository.AddNote(new Note
            {
                Id = newNote.Id,
                Body = newNote.Body,
                UpdatedOn = DateTime.Now,
                UserId = newNote.UserId

            });
        }

        //PUT api/note/5
        [HttpPut ("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            _noteRepository.UpdateNoteDocument(id, value);
        }

        //DELETE api/notes/23243423
        [HttpDelete ("{id}")]
        public void Delete(string id)
        {
            _noteRepository.RemoveNote(id);
        }
    }
}