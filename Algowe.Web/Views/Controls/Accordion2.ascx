<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div style="height:200px; display:block;">

<b>Accordion2</b>
<br /> <br />
This accordion item has been loaded statically using the Html.RenderPartail mechanism:
<br />

.AddAccordionItem(new AccordionItem()<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <b><i>.SetContent(ContetType.PartialView, "Accordion2")</i></b>
<br />
&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; .SetTitle("PartialView"))

</div>