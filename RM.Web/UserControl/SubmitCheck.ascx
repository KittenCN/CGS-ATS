<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubmitCheck.ascx.cs" Inherits="RM.Web.UserControl.SubmitCheck" %>
<%--防止刷新重复提交 txt_hiddenToken--%>
<input id="txt_hiddenToken" type="hidden" value="<%=GetToken() %>" name="txt_hiddenToken" />