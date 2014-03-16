using System;
using System.Linq;
using DemoProject.Model;

namespace DemoProject.API.Repositories
{
    /// <summary>
    /// IMetadataRepository implementation responsible for saving metatata in a database.
    /// </summary>
    public class MetadataDbRepository : IMetadataRepository, IDisposable
    {
        private readonly DemoProjectContext context;

        public MetadataDbRepository()
        {
            this.context = new DemoProjectContext();
        }

        public IQueryable<Metadata> GetAll()
        {
            return this.context.Metadata;
        }

        public void Delete(int id)
        {
            var metadataToRemove = this.context.Metadata.FirstOrDefault(x => x.Id == id);
            
            if (metadataToRemove != null)
            {
                this.context.Metadata.Remove(metadataToRemove);
                this.context.SaveChanges();
            }
        }

        public Metadata Save(Metadata metadata)
        {
            this.context.Metadata.Add(metadata);
            this.context.SaveChanges();

            return metadata;
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}