using System.ComponentModel;
using System.Threading.Tasks;
using Ape.Volo.Api.Controllers.Base;
using Ape.Volo.Common.Extensions;
using Ape.Volo.Common.Model;
using Ape.Volo.IBusiness.Dto.Message.Email;
using Ape.Volo.IBusiness.Interface.Message.Email;
using Ape.Volo.IBusiness.QueryModel;
using Ape.Volo.IBusiness.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace Ape.Volo.Api.Controllers.Message.Email;

/// <summary>
/// 邮件模板管理
/// </summary>
[Area("邮件模板管理")]
[Route("/api/email/template", Order = 18)]
public class EmailMessageTemplateController : BaseApiController
{
    #region 字段

    private readonly IEmailMessageTemplateService _emailMessageTemplateService;

    #endregion

    #region 构造函数

    public EmailMessageTemplateController(IEmailMessageTemplateService emailMessageTemplateService)
    {
        _emailMessageTemplateService = emailMessageTemplateService;
    }

    #endregion

    #region API

    /// <summary>
    /// 新增邮箱账户
    /// </summary>
    /// <param name="createUpdateEmailMessageTemplateDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("创建")]
    public async Task<ActionResult> Create(
        [FromBody] CreateUpdateEmailMessageTemplateDto createUpdateEmailMessageTemplateDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _emailMessageTemplateService.CreateAsync(createUpdateEmailMessageTemplateDto);
        return Ok(result);
    }

    /// <summary>
    /// 更新邮箱账户
    /// </summary>
    /// <param name="createUpdateEmailMessageTemplateDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("编辑")]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateEmailMessageTemplateDto createUpdateEmailMessageTemplateDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _emailMessageTemplateService.UpdateAsync(createUpdateEmailMessageTemplateDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除邮箱账户
    /// </summary>
    /// <param name="idCollection"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    [Description("删除")]
    public async Task<ActionResult> Delete([FromBody] IdCollection idCollection)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _emailMessageTemplateService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }

    /// <summary>
    /// 邮箱账户列表
    /// </summary>
    /// <param name="messageTemplateQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("查询")]
    public async Task<ActionResult> Query(EmailMessageTemplateQueryCriteria messageTemplateQueryCriteria,
        Pagination pagination)
    {
        var emailMessageTemplateList =
            await _emailMessageTemplateService.QueryAsync(messageTemplateQueryCriteria, pagination);

        return JsonContent(emailMessageTemplateList, pagination);
    }

    #endregion
}
