using API_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Data_Server.Data;
using Web_API_Data_Server.Models.TransferRequest;

namespace Web_API_Data_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        // Deposit function
        [HttpPost("deposit")]
        public IActionResult Deposit([FromQuery] uint accountNo, [FromQuery] decimal amount)
        {
            if (DBManager.Deposit(accountNo, amount))
            {
                return Ok("Successfully deposited");
            }
            else
            {
                return BadRequest("Error in deposit");
            }
        }



        // Withdraw function
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromQuery] uint accountNo, [FromQuery] decimal amount)
        {
            if (DBManager.Withdraw(accountNo, amount))
            {
                return Ok("Successfully withdrawn");
            }
            else
            {
                return BadRequest("Error in withdrawal");
            }
        }


        // Get all transactions
        [HttpGet("all")]
        public IActionResult GetAllTransactions()
        {
            var transactions = DBManager.GetAllTransactions();
            if (transactions != null && transactions.Count > 0)
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound("No transactions found");
            }
        }

        // Get transactions by accountNo
        [HttpGet("{accountNo}")]
        public IActionResult GetTransactionsByAccountNo(uint accountNo)
        {
            var transactions = DBManager.GetTransactionsByAccountNo(accountNo);
            if (transactions != null && transactions.Count > 0)
            {
                return Ok(transactions);
            }
            else
            {
                return NotFound($"No transactions found for account number {accountNo}");
            }
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferInputModel transferData)
        {
            if (DBManager.TransferMoney(transferData.SenderAccountNo, transferData.RecipientAccountNo, transferData.Amount, transferData.Description))
            {
                return Ok("Successfully transferred money");
            }
            else
            {
                return BadRequest("Error in money transfer");
            }
        }


    }
}
