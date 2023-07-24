using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstAzureWebApp.common;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("[controller]")]
    public class BarController : BaseController
    {

       
        [AllowAnonymous]
        [HttpGet("version")]
        public ContentResult version()
        {
            return Content(

                        "<html>" +
                        "<head>" +
                        "<meta  charset='UTF - 8' name='viewport' content='width=device-width, initial-scale=1'>" +
                        "<style>" +
                        ".card {" +
                        "  border-style: solid; border-width:thin; border-color: #9e9e9e;" +
                        "  transition: 0.3s;" +
                        "  width: 40%;" +
                        "}" +
                        ".card:hover {" +
                        "  border-style: solid; border-width:2px; border-color: #00bcd4;" +
                        "}" +
                        ".container {" +
                        "  padding: 2px 16px;" +
                        "}" +
                        "</style>" +
                        "</head>" +
                        "<body>" +
                        "<h2>API Current Version:" + CommonConstant.VERSION + "</h2>" +

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>17/01/2022 v1.0.2 </b></h4> " +
                        "    <p>1. Template Stock Card: ทำการเปลี่ยนแปลงสิทธิ์ในการเข้าถึงระดับ BU</p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>02/02/2022  v1.0.3 </b></h4> " +
                        "    <p>1. แก้ไขการ Mapping ตำบล โดยสามารถใช้ชื่อตำบลซ้ำแต่อยู่คนล่ะอำเภอได้</p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>23/02/2022  v2.0.0 </b></h4> " +
                        "    <p>1. Penetration Test </p> " +
                        "    <p>2. Sale Visit, Adhoc Template น่าจะต้องมีปุ่ม delete ให้ลบ template ที่ adhoc เข้าไป (จะแก้ไขตอนกดปุ่มแล้ว จะกดได้ครั้งเดียว ให้ปุ่มdisable) </p> " +
                        "    <p>3. Sale Order, Company แสดงซ้ำเยอะมาก อาจจะมีข้อมูลอะไรข้างหลังอยู่ ที่มันอาจจะแสดงความแตกต่างได้ :</p> " +
                        "    <p>4. Sale Visit Plan, Template for SA เมื่อไปกำหนดสถานะให้ ไม่ใช้งาน แล้ว ควรจะไม่แสดงที่หน้า Create Plan ของ Special Task</p> " +
                        "    <p>5. เรื่องแก้ query ที่กิน resources เยอะๆ 4 api searchMyAccount, searchProspectRecommend,searchAccountInTerritory,searchOtherProspect </p> " +
                        "    <p>6. CR: ทำหน้าจอเพื่อให้เลือกพิมพ์ QR Code ทั้งหมดของ Customer ที่เลือกในรูปแบบของ A4 (หลายๆ QR อยู่ใน A4 ตามจำนวนที่กำหนด เพื่อเอาไป Print) </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>16/03/2022  v2.0.1 </b></h4> " +
                        "    <p>1. Edit Service /addProspectDedicated รับ TerritoryId เป็น Multi(Array) </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>01/04/2022  v2.0.2 </b></h4> " +
                        "    <p>1. เพิ่ม Sale Group กับ Territory ในตารางแสดงผลของ Employee เมนู : Now </p> " +
                        "    <p>2. Service:/updSaleOrder แก้ไขการคำนวน NetPrice1 โดย Condition_Total_Value - (Sum(Condition_Amount)/Sum(Condition_Per))  ของ ZL34 </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>04/04/2022  v2.0.3 </b></h4> " +
                        "    <p>1. Fix bug Service:/updSaleOrder แก้ไขการคำนวน NetPrice1 โดย Condition_Total_Value - (Sum(Condition_Amount)/Sum(Condition_Per))  ของ ZL34 </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>16/11/2022  v2.0.4 </b></h4> " +
                        "    <p>1. Redesign </p> " +
                        "    <p>2. ตัด Table ORG_SALE_TERRITORY </p> " +
                        "    <p>3. แก้ไข Bug ต่างๆ </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>27/12/2022  v2.0.5 </b></h4> " +
                        "    <p>1. Update Service org/updrSaleGroupByManagerEmpId เพิ่มเงื่อนไข G.GROUP_CODE in 00005 เข้าไป </p> " +
                        "    <p>2. Update Service adm/searchAdmEmpRoleManager ตัดเงื่อนไข SG.ACTIVE_FLAG=Y ฮฮฏ</p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>27/12/2022  v2.0.7 </b></h4> " +
                        "    <p>1. Update Report </p> " +
                        "    <p>2. Update Service searchShipToByCustSaleId Field CUST_NAME_TH </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card

                        // Card
                        "<div class='card'> " +
                        "  <div class='container'>" +
                        "    <h4><b>16/03/2022  v2.0.8 </b></h4> " +
                        "    <p>1. Update Report </p> " +
                        "  </div>" +
                        "</div>" +
                        // Card




                        "</div>" +
                        "</body>" +
                        "</html> "

                       , "text/html", System.Text.Encoding.UTF8);


        }


    }

}
