using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FruitsController() : ControllerBase
{
    private static readonly List<Fruit> Fruits =
    [
        new("Banana"), new("Apple"), new("Pineapple")
    ];

    [HttpGet(Name = "GetAllFruits")]
    [ProducesResponseType(typeof(List<Fruit>), StatusCodes.Status200OK)]
    public ActionResult<List<Fruit>> GetAll()
    {
        return Ok(Fruits);
    }

    [HttpPost(Name = "CreateNewFruit")]
    [ProducesResponseType(typeof(Fruit), StatusCodes.Status201Created)]
    public ActionResult Create([FromBody] string fruit)
    {
        Fruit newFruit = new(fruit);
        Fruits.Add(newFruit);
        return CreatedAtAction(nameof(GetById), new { newFruit.Id }, newFruit);
    }

    [HttpGet("{id:guid}", Name = "GetFruitById")]
    [ProducesResponseType(typeof(Fruit), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Fruit> GetById([FromRoute] Guid id)
    {
        Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);
        return searchedFruit is null
            ? NotFound()
            : Ok(searchedFruit);
    }

    [HttpPut("{id:guid}", Name = "UpdateFruit")]
    [ProducesResponseType(typeof(Fruit), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Update([FromRoute] Guid id, [FromBody] string fruit)
    {
        Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);

        if (searchedFruit is null)
        {
            return NotFound();
        }

        searchedFruit.Name = fruit;
        return Ok();
    }

    [HttpDelete("{id:guid}", Name = "DeleteFruit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete([FromRoute] Guid id)
    {
        Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);
        
        if (searchedFruit is null)
        {
            return NotFound();
        }

        Fruits.Remove(searchedFruit);
        return NoContent();
    }
}
