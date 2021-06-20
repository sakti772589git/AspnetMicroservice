﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CatalogController : ControllerBase
  {
    private readonly IProductRepository _productRepo;
    private readonly ILogger _logger;

    public CatalogController(IProductRepository productRepo, ILogger logger)
    {
      _productRepo = productRepo;
      _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
      var products = await _productRepo.GetProducts();
      return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name ="GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
      var product = await _productRepo.GetProduct(id);
      if(product == null)
      {
        _logger.LogError($"Product with id: {id}, not found");
        return NotFound();
      }
      return Ok(product);
    }

    [Route("[action]/[name]", Name="GetProductByName")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
    {
      var products = await _productRepo.GetProductsByName(name);
      if(products == null)
      {
        _logger.LogError($"Products with name: {name}, not found");
        return NotFound();
      }
      return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> CreateProduct([FromBody] Product product)
    {
      await _productRepo.CreatedProduct(product);
      return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {      
      return Ok(await _productRepo.UpdateProduct(product));
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
      return Ok(await _productRepo.DeleteProduct(id));
    }

  }
}
