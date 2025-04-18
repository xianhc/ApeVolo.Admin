using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ape.Volo.Business.Base;
using Ape.Volo.Common;
using Ape.Volo.Common.Enums;
using Ape.Volo.Common.Extensions;
using Ape.Volo.Common.Model;
using Ape.Volo.Entity.System;
using Ape.Volo.IBusiness.Dto.System;
using Ape.Volo.IBusiness.ExportModel.System;
using Ape.Volo.IBusiness.Interface.System;
using Ape.Volo.IBusiness.QueryModel;

namespace Ape.Volo.Business.System;

/// <summary>
/// 租户服务
/// </summary>
public class TenantService : BaseServices<Tenant>, ITenantService
{
    #region 构造函数

    public TenantService()
    {
    }

    #endregion

    public async Task<OperateResult> CreateAsync(CreateUpdateTenantDto createUpdateTenantDtoDto)
    {
        if (await TableWhere(r => r.TenantId == createUpdateTenantDtoDto.TenantId).AnyAsync())
        {
            return OperateResult.Error($"租户Id=>{createUpdateTenantDtoDto.TenantId}=>已存在!");
        }

        if (createUpdateTenantDtoDto.TenantType == TenantType.Db)
        {
            if (createUpdateTenantDtoDto.DbType.IsNull())
            {
                return OperateResult.Error($"数据库类型不能为空");
            }

            if (createUpdateTenantDtoDto.ConfigId.IsNullOrEmpty())
            {
                return OperateResult.Error($"数据库标识ID不能为空");
            }

            if (createUpdateTenantDtoDto.Connection.IsNullOrEmpty())
            {
                return OperateResult.Error($"数据库连接不能为空");
            }

            if (await TableWhere(r => r.ConfigId == createUpdateTenantDtoDto.ConfigId).AnyAsync())
            {
                return OperateResult.Error($"标识Id=>{createUpdateTenantDtoDto.ConfigId}=>已存在!");
            }
        }

        var tenant = App.Mapper.MapTo<Tenant>(createUpdateTenantDtoDto);
        var result = await AddAsync(tenant);
        return OperateResult.Result(result);
    }

    public async Task<OperateResult> UpdateAsync(CreateUpdateTenantDto createUpdateTenantDtoDto)
    {
        //取出待更新数据
        var oldTenant = await TableWhere(x => x.Id == createUpdateTenantDtoDto.Id).FirstAsync();
        if (oldTenant.IsNull())
        {
            return OperateResult.Error("数据不存在！");
        }

        if (oldTenant.TenantId != createUpdateTenantDtoDto.TenantId &&
            await TableWhere(x => x.TenantId == createUpdateTenantDtoDto.TenantId).AnyAsync())
        {
            return OperateResult.Error($"租户Id=>{createUpdateTenantDtoDto.TenantId}=>已存在!");
        }

        if (createUpdateTenantDtoDto.TenantType == TenantType.Db)
        {
            if (createUpdateTenantDtoDto.DbType.IsNull())
            {
                return OperateResult.Error($"数据库类型不能为空");
            }

            if (createUpdateTenantDtoDto.ConfigId.IsNullOrEmpty())
            {
                return OperateResult.Error($"数据库标识ID不能为空");
            }

            if (createUpdateTenantDtoDto.Connection.IsNullOrEmpty())
            {
                return OperateResult.Error($"数据库连接不能为空");
            }

            if (oldTenant.ConfigId != createUpdateTenantDtoDto.ConfigId &&
                await TableWhere(x => x.ConfigId == createUpdateTenantDtoDto.ConfigId).AnyAsync())
            {
                return OperateResult.Error($"标识Id=>{createUpdateTenantDtoDto.ConfigId}=>已存在!");
            }
        }

        var tenant = App.Mapper.MapTo<Tenant>(createUpdateTenantDtoDto);
        var result = await UpdateAsync(tenant, null, x => x.TenantId);
        return OperateResult.Result(result);
    }

    public async Task<OperateResult> DeleteAsync(HashSet<long> ids)
    {
        var tenants = await TableWhere(x => ids.Contains(x.Id)).Includes(x => x.Users).ToListAsync();
        if (tenants.Any(x => x.Users != null && x.Users.Count != 0))
        {
            return OperateResult.Error("存在用户关联，请解除后再试！");
        }

        var result = await LogicDelete<Tenant>(x => ids.Contains(x.Id));
        return OperateResult.Result(result);
    }

    public async Task<List<TenantDto>> QueryAsync(TenantQueryCriteria tenantQueryCriteria, Pagination pagination)
    {
        var queryOptions = new QueryOptions<Tenant>
        {
            Pagination = pagination,
            ConditionalModels = tenantQueryCriteria.ApplyQueryConditionalModel()
        };
        return App.Mapper.MapTo<List<TenantDto>>(
            await TablePageAsync(queryOptions));
    }

    public async Task<List<TenantDto>> QueryAllAsync()
    {
        return App.Mapper.MapTo<List<TenantDto>>(
            await Table.ToListAsync());
    }


    public async Task<List<ExportBase>> DownloadAsync(TenantQueryCriteria tenantQueryCriteria)
    {
        var tenants = await TableWhere(tenantQueryCriteria.ApplyQueryConditionalModel()).ToListAsync();
        List<ExportBase> tenantExports = new List<ExportBase>();
        tenantExports.AddRange(tenants.Select(x => new TenantExport()
        {
            TenantId = x.TenantId,
            Name = x.Name,
            Description = x.Description,
            TenantType = x.TenantType,
            ConfigId = x.ConfigId,
            DbType = x.DbType,
            ConnectionString = x.ConnectionString,
            CreateTime = x.CreateTime
        }));
        return tenantExports;
    }
}
