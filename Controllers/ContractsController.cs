using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Dtos;
using Prototype.Models;

namespace Prototype.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    public class ContractsController : Controller
    {
        private readonly IContractRepo _repository;
        private readonly IMapper _mapper;

        // Constructor uses Dependency Injection to map
        public ContractsController(IContractRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/contracts
        [HttpGet]
        public ActionResult<IEnumerable<ContractReadDto>> GetAllContracts()
        {
            var contractItems = _repository.GetAllContracts();

            return Ok(_mapper.Map<IEnumerable<ContractReadDto>>(contractItems));
        }

        // GET api/contracts/{id}
        [HttpGet("{id}", Name = "GetContractById")]
        public ActionResult<CustomerReadDto> GetContractById(int id)
        {
            var contractItem = _repository.GetContractById(id);

            if (contractItem != null)
            {
                return Ok(_mapper.Map<ContractReadDto>(contractItem));
            }

            return NotFound();
        }

        // POST api/contracts/
        [HttpPost]
        public ActionResult<ContractReadDto> CreateContract(ContractCreateDto contractCreateDto)
        {
            // TODO: Create a function that checks if CustomerID exists

            var contractModel = _mapper.Map<Contract>(contractCreateDto);
            _repository.CreateContract(contractModel);
            _repository.SaveChanges();

            var contractReadDto = _mapper.Map<ContractReadDto>(contractModel);

            return CreatedAtRoute(nameof(GetContractById), new { Id = contractModel.Id }, contractReadDto);
        }
    }
}