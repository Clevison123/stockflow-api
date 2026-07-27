using FluentValidation;
using StockFlow.Application.DTOs.Audit;
using StockFlow.Application.DTOs.Quality.CustomerClaim;
using StockFlow.Application.Exceptions;
using StockFlow.Application.Interfaces.IAudit;
using StockFlow.Application.Interfaces.Quality;
using StockFlow.Application.Interfaces.Quality.IServices;
using StockFlow.Application.Interfaces.Sales;
using StockFlow.Domain.Entities.Quality;
using StockFlow.Domain.Enums.Audit;
using System.Text.Json;

namespace StockFlow.Application.Services.Quality
{
    public class CustomerClaimService : ICustomerClaimService
    {
        private readonly ICustomerClaimRepository _customerClaimRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IAuditService _auditService;
        private readonly IValidator<CreateCustomerClaimDto> _createValidator;
        private readonly IValidator<UpdateCustomerClaimDto> _updateValidator;
        private readonly IValidator<ResolveCustomerClaimDto> _resolveValidator;



        public CustomerClaimService(
            ICustomerClaimRepository customerClaimRepository,
            ICustomerRepository customerRepository,
            ISalesOrderRepository salesOrderRepository,
            IAuditService auditService,
            IValidator<CreateCustomerClaimDto> createValidator,
            IValidator<UpdateCustomerClaimDto> updateValidator,
            IValidator<ResolveCustomerClaimDto> resolveValidator)
        {
            _customerClaimRepository = customerClaimRepository;
            _customerRepository = customerRepository;
            _salesOrderRepository = salesOrderRepository;
            _auditService = auditService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _resolveValidator = resolveValidator;
        }

        public async Task<CustomerClaimResponseDto> CreateAsync(CreateCustomerClaimDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(
                    validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with ID {dto.CustomerId} was not found.");
            }

            var salesOrder = await _salesOrderRepository.GetByIdAsync(dto.SalesOrderId);

            if (salesOrder is null)
            {
                throw new NotFoundException($"Sales order with ID {dto.SalesOrderId} was not found.");
            }

            if (salesOrder.CustomerId != dto.CustomerId)
            {
                throw new BusinessRuleException(
                    "The sales order does not belong to the specified customer.");
            }

            var claim = new CustomerClaim
            {
                CustomerId = dto.CustomerId,
                SalesOrderId = dto.SalesOrderId,
                ClaimType = dto.ClaimType,
                Description = dto.Description,
                ReportedAt = DateTime.UtcNow,
                IsResolved = false
            };

            await _customerClaimRepository.AddAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Create,
                Entity = AuditEntity.CustomerClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });

            return await GetByIdAsync(claim.Id);
        }

        public async Task<IEnumerable<CustomerClaimResponseDto>> GetAllAsync()
        {
            var customerClaims = await _customerClaimRepository.GetAllAsync();

            return customerClaims.Select(claim => new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            });
        }

        public async Task<IEnumerable<CustomerClaimResponseDto>> GetByCustomerAsync(int customerId)
        {
            if (customerId <= 0)
            {
                throw new BadRequestException("Invalid customer ID.");
            }

            var customerClaims = await _customerClaimRepository.GetByCustomerIdAsync(customerId);

            if (!customerClaims.Any())
            {
                throw new NotFoundException($"No customer claims found for customer ID {customerId}.");
            }

            return customerClaims.Select(claim => new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            });
        }

        public async Task<CustomerClaimResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer claim id.");
            }

            var claim = await _customerClaimRepository.GetByIdAsync(id);

            if (claim is null)
            {
                throw new NotFoundException($"Customer claim with ID {id} was not found.");
            }

            return new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            };
        }

        public async Task<IEnumerable<CustomerClaimResponseDto>> GetBySalesOrderAsync(int salesOrderId)
        {
            if (salesOrderId <= 0)
            {
                throw new BadRequestException("Invalid sales order ID.");
            }

            var salesOrder = await _salesOrderRepository.GetByIdAsync(salesOrderId);

            if (salesOrder is null)
            {
                throw new NotFoundException($"Sales order with ID {salesOrderId} was not found.");
            }

            var claims = await _customerClaimRepository.GetBySalesOrderIdAsync(salesOrderId);

            return claims.Select(claim => new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            });
        }

        public async Task<IEnumerable<CustomerClaimResponseDto>> GetOpenClaimsAsync()
        {
            var claims = await _customerClaimRepository.GetOpenClaimsAsync();

            if (!claims.Any())
            {
                throw new NotFoundException("No open customer claims were found.");
            }

            return claims.Select(claim => new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            });
        }

        public async Task<IEnumerable<CustomerClaimResponseDto>> GetResolvedClaimsAsync()
        {
            var claims = await _customerClaimRepository.GetResolvedClaimsAsync();

            if (!claims.Any())
            {
                throw new NotFoundException("No resolved customer claims were found.");
            }

            return claims.Select(claim => new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            });
        }

        public async Task<CustomerClaimResponseDto> ResolveAsync(int claimId, ResolveCustomerClaimDto dto)
        {
            if (claimId <= 0)
            {
                throw new BadRequestException("Invalid customer claim ID.");
            }

            var validationResult = await _resolveValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(
                    validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }

            var claim = await _customerClaimRepository.GetByIdAsync(claimId);

            if (claim is null)
            {
                throw new NotFoundException($"Customer claim with ID {claimId} was not found.");
            }

            if (claim.IsResolved)
            {
                throw new BadRequestException("Customer claim is already resolved.");
            }

            claim.IsResolved = true;
            claim.ResolvedAt = DateTime.UtcNow;
            claim.ResolutionNotes = dto.ResolutionNotes;

            await _customerClaimRepository.UpdateAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.CustomerClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });

            return new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            };
        }

        public async Task<CustomerClaimResponseDto> UpdateAsync(int id, UpdateCustomerClaimDto dto)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid customer claim ID.");
            }

            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ApplicationValidationException(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            var claim = await _customerClaimRepository.GetByIdAsync(id);

            if (claim is null)
            {
                throw new NotFoundException($"Customer claim with ID {id} was not found.");
            }

            claim.ClaimType = dto.ClaimType;
            claim.Description = dto.Description;

            await _customerClaimRepository.UpdateAsync(claim);

            await _auditService.CreateAuditLogAsync(new CreateAuditLogDto
            {
                Action = AuditAction.Update,
                Entity = AuditEntity.CustomerClaim,
                EntityId = claim.Id.ToString(),
                NewValues = JsonSerializer.Serialize(claim),
                Success = true
            });

            return new CustomerClaimResponseDto
            {
                Id = claim.Id,
                CustomerId = claim.CustomerId,
                CustomerName = claim.Customer.TradeName,
                SalesOrderId = claim.SalesOrderId,
                OrderNumber = claim.SalesOrder.OrderNumber,
                ClaimType = claim.ClaimType,
                Description = claim.Description,
                ReportedAt = claim.ReportedAt,
                IsResolved = claim.IsResolved,
                ResolvedAt = claim.ResolvedAt,
                ResolutionNotes = claim.ResolutionNotes
            };
        }
    }
}
