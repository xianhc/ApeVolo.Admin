﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Ape.Volo.Common.Model;
using Ape.Volo.Entity.Message.Email;
using Ape.Volo.IBusiness.Base;
using Ape.Volo.IBusiness.Dto.Message.Email;
using Ape.Volo.IBusiness.QueryModel;

namespace Ape.Volo.IBusiness.Interface.Message.Email;

/// <summary>
/// 邮件消息模板接口
/// </summary>
public interface IEmailMessageTemplateService : IBaseServices<EmailMessageTemplate>
{
    #region 基础接口

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateEmailMessageTemplateDto"></param>
    /// <returns></returns>
    Task<OperateResult> CreateAsync(CreateUpdateEmailMessageTemplateDto createUpdateEmailMessageTemplateDto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateEmailMessageTemplateDto"></param>
    /// <returns></returns>
    Task<OperateResult> UpdateAsync(CreateUpdateEmailMessageTemplateDto createUpdateEmailMessageTemplateDto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<OperateResult> DeleteAsync(HashSet<long> ids);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="messageTemplateQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<List<EmailMessageTemplateDto>> QueryAsync(EmailMessageTemplateQueryCriteria messageTemplateQueryCriteria,
        Pagination pagination);

    #endregion
}
