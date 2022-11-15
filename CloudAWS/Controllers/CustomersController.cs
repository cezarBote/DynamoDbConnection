using CloudAWS.Models;
using CloudAWS.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;
using System.Text.Json;

namespace CloudAWS.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> CreateNew(CustomerRequest request)
        {
            var customer = new Customer();
            customer.InjectFrom(request);
            customer.Id= Guid.NewGuid();
            await _customerService.CreateAsync(customer);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var customer = await _customerService.GetAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        public async Task<IActionResult> EditReq(Customer request)
        {
            var existingCustomer = await _customerService.GetAsync(request.Id);

            if (existingCustomer is null)
            {
                return NotFound();
            }

            var customer = new Customer();
            customer.InjectFrom(request);
            customer.Id = request.Id;
            await _customerService.UpdateAsync(customer);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _customerService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}
