using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;
using NotebookAppApi.Interfaces;
using NotebookAppApi.Model;

namespace NotebookAppApi.Data
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context = null;

        //public NoteRepository for settings
        public NoteRepository(IOptions<Settings> settings)
        {
            _context = new NoteContext(settings);
        }

        //The access to database will be asynchronous. We are using here the new driver, which offers a full async stack.

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            try
            {
                return await _context.Notes.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                //lor or manage exception
                throw ex;
            }
        }

        //query after Id or InternalId (BSonId value)
        public async Task<Note> GetNote(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Notes
                                .Find(note => note.Id == id || note.InternalId == internalId)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }

        }


        //query after body text, update time and header image size
        public async Task<IEnumerable<Note>> GetNote(string bodyText, DateTime updatedFrom, long headerSizeLimit)
        {
            try
            {
                var query = _context.Notes.Find(note => note.Body.Contains(bodyText) &&
                                       note.UpdatedOn >= updatedFrom &&
                                       note.HeaderImage.ImageSize <= headerSizeLimit);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                //log or manage the exception
                throw ex;
            }
        }

        //try to convert the Id to a BSonId value
        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        // add Note
        public async Task AddNote(Note item)
        {
            try
            {
                await _context.Notes.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                //log or manage the exception
                throw ex;
            }
        }

        // remove Note
        public async Task<bool> RemoveNote(string id)
        {
            try
            {
                DeleteResult actionResult = await _context.Notes.DeleteOneAsync(Builders<Note>.Filter.Eq("Id", id));
                return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
