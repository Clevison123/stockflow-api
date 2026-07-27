using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Sales.Customers;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Sales;
using StockFlow.Application.Interfaces.Sales.IServices;
using StockFlow.Domain.Entities.Sales;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Sales
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuditService _auditService;

        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;


        public CustomerService(
            ICustomerRepository customerRepository,
            IAuditService auditService,
            IValidator<CreateCustomerDto> createValidator,
            IValidator<UpdateCustomerDto> updateValidator)
        {
            _customerRepository = customerRepository;
            _auditService = auditService;

            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            if (await _customerRepository.CnpjExistsAsync(dto.Cnpj))
            {
                throw new ConflictException($"A customer with CNPJ {dto.Cnpj} already exists.");
            }

            if (await _customerRepository.EmailExistsAsync(dto.Email))
            {
                throw new ConflictException($"A customer with email {dto.Email} already exists.");
            }

            var customer = new Customer
            {
                TradeName = dto.TradeName,
                CompanyName = dto.CompanyName,
                Cnpj = dto.Cnpj,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                IsActive = true
            };

            await _customerRepository.AddAsync(customer);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.Customer,
                EntityId = customer.Id.ToString(),
                NewValues = JsonSerializer.Serialize(customer),
                Success = true
            });

            return await GetByIdAsync(customer.Id);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync(null);

            if (!customers.Any())
            {
                throw new NotFoundException("No customers were found.");
            }

            return customers.Select(customer => new CustomerResponseDto
            {
                Id = customer.Id,
                TradeName = customer.TradeName,
                CompanyName = customer.CompanyName,
                Cnpj = customer.Cnpj,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostalCode = customer.PostalCode,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            });
        }

        public async Task<CustomerResponseDto> GetByCnpjAsync(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                throw new BadRequestException("CNPJ is required.");
            }

            var customer = await _customerRepository.GetByCnpjAsync(cnpj);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with CNPJ {cnpj} was not found.");
            }

            return new CustomerResponseDto
            {
                Id = customer.Id,
                TradeName = customer.TradeName,
                CompanyName = customer.CompanyName,
                Cnpj = customer.Cnpj,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostalCode = customer.PostalCode,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

        public async Task<CustomerResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {id} was not found.");
            }

            return new CustomerResponseDto
            {
                Id = customer.Id,
                TradeName = customer.TradeName,
                CompanyName = customer.CompanyName,
                Cnpj = customer.Cnpj,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostalCode = customer.PostalCode,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

        public async Task<CustomerResponseDto> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {id} was not found.");
            }

            if (await _customerRepository.EmailExistsAsync(dto.Email, id))
            {
                throw new ConflictException($"Email '{dto.Email}' is already registered.");
            }

            customer.TradeName = dto.TradeName;
            customer.CompanyName = dto.CompanyName;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            customer.Address = dto.Address;
            customer.City = dto.City;
            customer.State = dto.State;
            customer.PostalCode = dto.PostalCode;

            await _customerRepository.UpdateAsync(customer);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Customer,
                EntityId = customer.Id.ToString(),
                NewValues = JsonSerializer.Serialize(customer),
                Success = true
            });

            return new CustomerResponseDto
            {
                Id = customer.Id,
                TradeName = customer.TradeName,
                CompanyName = customer.CompanyName,
                Cnpj = customer.Cnpj,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                PostalCode = customer.PostalCode,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }
        public async Task ActivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {id} was not found.");
            }

            if (customer.IsActive)
            {
                throw new BadRequestException("Customer is already active.");
            }

            customer.IsActive = true;

            await _customerRepository.UpdateAsync(customer);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Customer,
                EntityId = customer.Id.ToString(),
                NewValues = JsonSerializer.Serialize(customer),
                Success = true
            });
        }
        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {id} was not found.");
            }

            if (!customer.IsActive)
            {
                throw new BadRequestException("Customer is already inactive.");
            }

            customer.IsActive = false;

            await _customerRepository.UpdateAsync(customer);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.Customer,
                EntityId = customer.Id.ToString(),
                NewValues = JsonSerializer.Serialize(customer),
                Success = true
            });
        }

    }
}
