using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;

namespace NotebookAppApi.Data
{
    public class NoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes();
        Task<Note> GetNote(string id);

        //query after multiple parameters
        Task<IEnumerable<Note>> GetNote(string bodyText, DateTime updateFrom, long headerSizeLimit);

        //add new note document
        
    }
}
