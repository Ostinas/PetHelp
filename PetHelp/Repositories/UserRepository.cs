using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Models;

namespace PetHelp.Repositories
{
    public class UserRepository
    {
        private readonly PetHelpContext _context;

        public UserRepository(PetHelpContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<int> PutUser(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return -1;
                }
                else
                {
                    throw;
                }
            }

            return 1;
        }

        public async Task<int> PostUser(User user)
        {
            _context.User.Add(user);
            var id = await _context.SaveChangesAsync();

            return id;
        }

        public async Task<int> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return -1;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return 1;
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
