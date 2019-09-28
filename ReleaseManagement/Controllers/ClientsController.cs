﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using ReleaseManagement.Core.Models;
using ReleaseManagement.Data.Context;

namespace ReleaseManagement.Controllers {
    public class ClientsController : ApiController {
        private ReleaseManagementContext db = new ReleaseManagementContext();

        // GET: api/Clients
        public IHttpActionResult GetClients() {
            var clients = db.Clients.OrderBy(p => p.ClientName).ToList();
            return Ok(clients);
        }

        // GET: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClient(int id) {
            Client client = db.Clients.Find(id);
            if (client == null) {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId) {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!ClientExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Clients
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Clients.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id) {
            Client client = db.Clients.Find(id);
            if (client == null) {
                return NotFound();
            }

            db.Clients.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id) {
            return db.Clients.Count(e => e.ClientId == id) > 0;
        }
    }
}