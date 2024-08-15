using API_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Data_Server.Models.Account;
using System.Linq;
using Web_API_Data_Server.Data;

namespace Web_API_Data_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Account> GetAccounts()
        {
            List<Account> accounts = DBManager.GetAll();
            return accounts;
        }

        [HttpGet]
        [Route("{accountNo}")]
        public IActionResult Get(uint accountNo)
        {
            Account account = DBManager.GetByAccountNo(accountNo);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Account account)
        {
            if (DBManager.Insert(account))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }

        [HttpDelete]
        [Route("{accountNo}")]
        public IActionResult Delete(uint accountNo)
        {
            if (DBManager.Delete(accountNo))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }

        [HttpPut("{accountNo}")]
        public IActionResult Update(uint accountNo, [FromBody] Account account)
        {
            if (DBManager.Update(account))
            {
                return Ok("Successfully updated");
            }
            return BadRequest("Could not update");
        }

       /* [HttpGet("count")]
        public ActionResult<int> GetNumEntries()
        {
            return DBManager.CountAll();
        }*/

    }
}



