namespace SPDC.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SPDC.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SPDC.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = Int32.MaxValue;
        }

        protected override void Seed(SPDC.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //CreateUser(context);
            //CreateLanguage(context);
            //CreateCourseType(context);
            //CreateCourseCategories(context);
            //CreateCourseLocations(context);
            //CreateCmsContentTypes(context);
            //CreateLevelofApprovals(context);
            //CreateLectures(context);
            //CreateProgrammeLeaders(context);
            //CreateMediumOfInstructions(context);
            //CreateClassCommon(context);
            //CreateClients(context);
            //CreateEnrnollmentStatus(context);
            //CreateInvoiceStatus(context);
            //CreateInvoiceItemType(context);
            //CreateTransactionType(context);
            //CreateAcceptedBank(context);
            //CreateCommonData(context);
        }

        private void CreateCommonData(ApplicationDbContext context)
        {
            if (!context.CommonData.Any())
            {
                context.CommonData.Add(new CommonData()
                {
                    Name = "Email Serial Munber",
                    Key = "",
                    ValueDouble = 0,
                    ValueInt = 1,
                    ValueString = ""
                });
            }
        }

        private void CreateAcceptedBank(ApplicationDbContext context)
        {
            if (!context.AcceptedBanks.Any())
            {
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "-",
                    NameCN = "-",
                    NameHK = "-"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "003 -Standard Chartered Bank (Hong Kong) Limited",
                    NameCN = "003 -渣打銀行(香港)有限公司",
                    NameHK = "003 -渣打銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "004 -The Hongkong and Shanghai Banking Corporation Limited",
                    NameCN = "004 -香港上海滙豐銀行有限公司",
                    NameHK = "004 -香港上海滙豐銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "005 -Credit Agricole Corporate and Investment Bank",
                    NameCN = "005 -東方匯理銀行",
                    NameHK = "005 -東方匯理銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "006 -Citibank, N.A.",
                    NameCN = "006 -花旗銀行",
                    NameHK = "006 -花旗銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "007 -JPMorgan Chase Bank, N.A.",
                    NameCN = "007 -摩根大通銀行",
                    NameHK = "007 -摩根大通銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "008 -The Royal Bank of Scotland plc",
                    NameCN = "008 -蘇格蘭皇家銀行有限公司",
                    NameHK = "008 -蘇格蘭皇家銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "009 -China Construction Bank (Asia) Corporation Limited",
                    NameCN = "009 -中國建設銀行(亞洲)股份有限公司",
                    NameHK = "009 -中國建設銀行(亞洲)股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "012 -Bank of China (Hong Kong) Limited",
                    NameCN = "012 -中國銀行(香港)有限公司",
                    NameHK = "012 -中國銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "015 -The Bank of East Asia, Limited",
                    NameCN = "015 -東亞銀行有限公司",
                    NameHK = "015 -東亞銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "016 -DBS Bank (Hong Kong) Limited",
                    NameCN = "016 -星展銀行(香港)有限公司",
                    NameHK = "016 -星展銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "018 -China CITIC Bank International Limited",
                    NameCN = "018 -中信銀行國際有限公司",
                    NameHK = "018 -中信銀行國際有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "020 -Wing Lung Bank Ltd.",
                    NameCN = "020 -永隆銀行有限公司",
                    NameHK = "020 -永隆銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "022 -Oversea-Chinese Banking Corporation Ltd.",
                    NameCN = "022 -華僑銀行",
                    NameHK = "022 -華僑銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "024 -Hang Seng Bank Ltd.",
                    NameCN = "024 -恒生銀行有限公司",
                    NameHK = "024 -恒生銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "025 -Shanghai Commercial Bank Ltd.",
                    NameCN = "025 -上海商業銀行有限公司",
                    NameHK = "025 -上海商業銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "027 -Bank of Communications Co., Ltd.",
                    NameCN = "027 -交通銀行股份有限公司",
                    NameHK = "027 -交通銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "028 -Public Bank (Hong Kong) Limited",
                    NameCN = "028 -大衆銀行(香港)有限公司",
                    NameHK = "028 -大衆銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "035 -OCBC Wing Hang Bank Limited",
                    NameCN = "035 -華僑永亨銀行有限公司",
                    NameHK = "035 -華僑永亨銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "038 -Tai Yau Bank Ltd.",
                    NameCN = "038 -大有銀行有限公司",
                    NameHK = "038 -大有銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "039 -Chiyu Banking Corporation Ltd.",
                    NameCN = "039 -集友銀行有限公司",
                    NameHK = "039 -集友銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "040 -Dah Sing Bank, Ltd.",
                    NameCN = "040 -大新銀行有限公司",
                    NameHK = "040 -大新銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "041 -Chong Hing Bank Limited",
                    NameCN = "041 -創興銀行有限公司",
                    NameHK = "041 -創興銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "043 -Nanyang Commercial Bank, Ltd.",
                    NameCN = "043 -南洋商業銀行有限公司",
                    NameHK = "043 -南洋商業銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "045 -UCO Bank",
                    NameCN = "045 -UCO Bank",
                    NameHK = "045 -UCO Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "046 -KEB Hana Bank",
                    NameCN = "046 -KEB Hana Bank",
                    NameHK = "046 -KEB Hana Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "047 -The Bank of Tokyo-Mitsubishi UFJ, Ltd.",
                    NameCN = "047 -三菱東京 UFJ 銀行",
                    NameHK = "047 -三菱東京 UFJ 銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "049 -Bangkok Bank Public Company Limited",
                    NameCN = "049 -盤谷銀行",
                    NameHK = "049 -盤谷銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "050 -Indian Overseas Bank",
                    NameCN = "050 -印度海外銀行",
                    NameHK = "050 -印度海外銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "054 -Deutsche Bank AG",
                    NameCN = "054 -德意志銀行",
                    NameHK = "054 -德意志銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "055 -Bank of America, N.A.",
                    NameCN = "055 -美國銀行",
                    NameHK = "055 -美國銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "056 -BNP Paribas",
                    NameCN = "056 -法國巴黎銀行",
                    NameHK = "056 -法國巴黎銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "058 -Bank of India",
                    NameCN = "058 -印度銀行",
                    NameHK = "058 -印度銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "060 -National Bank of Pakistan",
                    NameCN = "060 -巴基斯坦國民銀行",
                    NameHK = "060 -巴基斯坦國民銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "061 -Tai Sang Bank Limited",
                    NameCN = "061 -大生銀行有限公司",
                    NameHK = "061 -大生銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "063 -Malayan Banking Berhad (Maybank)",
                    NameCN = "063 -馬來亞銀行",
                    NameHK = "063 -馬來亞銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "065 -Sumitomo Mitsui Banking Corporation",
                    NameCN = "065 -三井住友銀行",
                    NameHK = "065 -三井住友銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "066 -PT. Bank Negara Indonesia (Persero) Tbk.",
                    NameCN = "066 -印尼國家銀行",
                    NameHK = "066 -印尼國家銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "067 -BDO Unibank, Inc.",
                    NameCN = "067 -金融銀行有限公司",
                    NameHK = "067 -金融銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "071 -United Overseas Bank Ltd.",
                    NameCN = "071 -大華銀行有限公司",
                    NameHK = "071 -大華銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "072 -Industrial and Commercial Bank of China (Asia) Limited",
                    NameCN = "072 -中國工商銀行(亞洲)有限公司",
                    NameHK = "072 -中國工商銀行(亞洲)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "074 -Barclays Bank Plc.",
                    NameCN = "074 -Barclays Bank Plc.",
                    NameHK = "074 -Barclays Bank Plc."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "076 -The Bank of Nova Scotia",
                    NameCN = "076 -加拿大豐業銀行",
                    NameHK = "076 -加拿大豐業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "080 -Royal Bank of Canada",
                    NameCN = "080 -加拿大皇家銀行",
                    NameHK = "080 -加拿大皇家銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "081 -Societe Generale",
                    NameCN = "081 -法國興業銀行",
                    NameHK = "081 -法國興業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "082 -State Bank of India",
                    NameCN = "082 -印度國家銀行",
                    NameHK = "082 -印度國家銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "085 -The Toronto-Dominion Bank",
                    NameCN = "085 -加拿大多倫多道明銀行",
                    NameHK = "085 -加拿大多倫多道明銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "086 -Bank of Montreal",
                    NameCN = "086 -滿地可銀行",
                    NameHK = "086 -滿地可銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "092 -Canadian Imperial Bank of Commerce",
                    NameCN = "092 -加拿大帝國商業銀行",
                    NameHK = "092 -加拿大帝國商業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "097 -Commerzbank AG",
                    NameCN = "097 -德國商業銀行",
                    NameHK = "097 -德國商業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "103 -UBS AG, Hong Kong",
                    NameCN = "103 -瑞士銀行",
                    NameHK = "103 -瑞士銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "106 -HSBC Bank USA, N.A.",
                    NameCN = "106 -美國滙豐銀行",
                    NameHK = "106 -美國滙豐銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "109 -Mizuho Bank, Ltd., Hong Kong Branch",
                    NameCN = "109 -瑞穗銀行",
                    NameHK = "109 -瑞穗銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "113 -DZ BANK AG Deutsche Zentral-Genossenschaftsbank",
                    NameCN = "113 -德國中央合作銀行",
                    NameHK = "113 -德國中央合作銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "118 -Woori Bank",
                    NameCN = "118 -友利銀行",
                    NameHK = "118 -友利銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "119 -Philippine National Bank",
                    NameCN = "119 -Philippine National Bank",
                    NameHK = "119 -Philippine National Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "128 -Fubon Bank (Hong Kong) Limited",
                    NameCN = "128 -富邦銀行(香港)有限公司",
                    NameHK = "128 -富邦銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "138 -Mitsubishi UFJ Trust and Banking Corporation",
                    NameCN = "138 -三菱 UFJ 信託銀行",
                    NameHK = "138 -三菱 UFJ 信託銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "139 -The Bank of New York Mellon",
                    NameCN = "139 -紐約梅隆銀行有限公司",
                    NameHK = "139 -紐約梅隆銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "145 -ING Bank N.V.",
                    NameCN = "145 -ING Bank N.V.",
                    NameHK = "145 -ING Bank N.V."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "147 -Banco Bilbao Vizcaya Argentaria, S.A.",
                    NameCN = "147 -西班牙對外銀行",
                    NameHK = "147 -西班牙對外銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "150 -National Australia Bank Limited",
                    NameCN = "150 -澳洲銀行",
                    NameHK = "150 -澳洲銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "151 -Westpac Banking Corporation",
                    NameCN = "151 -西太平洋銀行",
                    NameHK = "151 -西太平洋銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "152 -Australia and New Zealand Banking Group Ltd.",
                    NameCN = "152 -澳洲紐西蘭銀行集團",
                    NameHK = "152 -澳洲紐西蘭銀行集團"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "153 -Commonwealth Bank of Australia",
                    NameCN = "153 -澳洲聯邦銀行",
                    NameHK = "153 -澳洲聯邦銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "161 -Intesa Sanpaolo S.p.A.",
                    NameCN = "161 -Intesa Sanpaolo S.p.A.",
                    NameHK = "161 -Intesa Sanpaolo S.p.A."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "164 -UniCredit Bank AG",
                    NameCN = "164 -裕信銀行",
                    NameHK = "164 -裕信銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "165 -Svenska Handelsbanken AB (publ)",
                    NameCN = "165 -瑞典商業銀行",
                    NameHK = "165 -瑞典商業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "170 -The Chiba Bank Ltd.",
                    NameCN = "170 -千葉銀行",
                    NameHK = "170 -千葉銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "178 -KBC Bank N.V., Hong Kong Branch",
                    NameCN = "178 -比利時聯合銀行",
                    NameHK = "178 -比利時聯合銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "180 -Wells Fargo Bank, N.A., Hong Kong Branch",
                    NameCN = "180 -富國銀行香港分行",
                    NameHK = "180 -富國銀行香港分行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "183 -Coöperatieve Rabobank U.A.",
                    NameCN = "183 -荷蘭合作銀行",
                    NameHK = "183 -荷蘭合作銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "185 -DBS Bank Ltd, Hong Kong Branch",
                    NameCN = "185 -星展銀行香港分行",
                    NameHK = "185 -星展銀行香港分行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "186 -The Shizuoka Bank Ltd.",
                    NameCN = "186 -靜岡銀行",
                    NameHK = "186 -靜岡銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "188 -The Hachijuni Bank, Ltd.",
                    NameCN = "188 -八十二銀行",
                    NameHK = "188 -八十二銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "198 -Hua Nan Commercial Bank, Ltd.",
                    NameCN = "198 -華南商業銀行股份有限公司",
                    NameHK = "198 -華南商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "199 -The Shiga Bank, Ltd.",
                    NameCN = "199 -滋賀銀行",
                    NameHK = "199 -滋賀銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "201 -Bank of Taiwan",
                    NameCN = "201 -臺灣銀行股份有限公司",
                    NameHK = "201 -臺灣銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "202 -The Chugoku Bank Limited",
                    NameCN = "202 -The Chugoku Bank Limited",
                    NameHK = "202 -The Chugoku Bank Limited"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "203 -First Commercial Bank",
                    NameCN = "203 -第一商業銀行股份有限公司",
                    NameHK = "203 -第一商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "205 -Coutts & Co. Ltd.",
                    NameCN = "205 -Coutts & Co. Ltd.",
                    NameHK = "205 -Coutts & Co. Ltd."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "206 -Chang Hwa Commercial Bank, Ltd.",
                    NameCN = "206 -彰化商業銀行股份有限公司",
                    NameHK = "206 -彰化商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "210 -Natixis",
                    NameCN = "210 -法國外貿銀行",
                    NameHK = "210 -法國外貿銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "214 -Industrial and Commercial Bank of China Limited",
                    NameCN = "214 -中國工商銀行股份有限公司",
                    NameHK = "214 -中國工商銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "220 -State Street Bank and Trust Company",
                    NameCN = "220 -美國道富銀行",
                    NameHK = "220 -美國道富銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "221 -China Construction Bank Corporation",
                    NameCN = "221 -中國建設銀行股份有限公司",
                    NameHK = "221 -中國建設銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "222 -Agricultural Bank of China Limited",
                    NameCN = "222 -中國農業銀行股份有限公司",
                    NameHK = "222 -中國農業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "224 -The Iyo Bank, Ltd., Hong Kong Branch",
                    NameCN = "224 -伊予銀行",
                    NameHK = "224 -伊予銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "227 -Erste Group Bank AG",
                    NameCN = "227 -Erste Group Bank AG",
                    NameHK = "227 -Erste Group Bank AG"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "229 -CTBC Bank Co., Ltd.",
                    NameCN = "229 -中國信託商業銀行股份有限公司",
                    NameHK = "229 -中國信託商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "230 -Taiwan Business Bank, Hong Kong Branch",
                    NameCN = "230 -臺灣中小企業銀行",
                    NameHK = "230 -臺灣中小企業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "233 -Credit Suisse AG",
                    NameCN = "233 -Credit Suisse AG",
                    NameHK = "233 -Credit Suisse AG"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "234 -Banca Monte dei Paschi di Siena S.p.A. Hong Kong Branch",
                    NameCN = "234 -意大利西雅那銀行香港分行",
                    NameHK = "234 -意大利西雅那銀行香港分行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "235 -HSBC Private Bank (Suisse) SA",
                    NameCN = "235 -滙豐私人銀行(瑞士)有限公司",
                    NameHK = "235 -滙豐私人銀行(瑞士)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "236 -Cathay United Bank Company, Limited",
                    NameCN = "236 -國泰世華商業銀行股份有限公司",
                    NameHK = "236 -國泰世華商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "237 -EFG Bank AG",
                    NameCN = "237 -瑞士盈豐銀行股份有限公司",
                    NameHK = "237 -瑞士盈豐銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "238 -China Merchants Bank Co., Ltd.",
                    NameCN = "238 -招商銀行股份有限公司",
                    NameHK = "238 -招商銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "239 -Taipei Fubon Commercial Bank Co., Ltd.",
                    NameCN = "239 -台北富邦商業銀行股份有限公司",
                    NameHK = "239 -台北富邦商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "241 -Bank SinoPac",
                    NameCN = "241 -永豐商業銀行股份有限公司",
                    NameHK = "241 -永豐商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "242 -Mega International Commercial Bank Co., Ltd.",
                    NameCN = "242 -兆豐國際商業銀行",
                    NameHK = "242 -兆豐國際商業銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "243 -E.Sun Commercial Bank, Ltd.",
                    NameCN = "243 -玉山商業銀行股份有限公司",
                    NameHK = "243 -玉山商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "245 -Taishin International Bank Co., Ltd.",
                    NameCN = "245 -台新國際商業銀行股份有限公司",
                    NameHK = "245 -台新國際商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "248 -Hong Leong Bank Berhad",
                    NameCN = "248 -豐隆銀行有限公司",
                    NameHK = "248 -豐隆銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "249 -Standard Chartered Bank Hong Kong Branch",
                    NameCN = "249 -渣打銀行",
                    NameHK = "249 -渣打銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "250 -Citibank (Hong Kong) Limited",
                    NameCN = "250 -花旗銀行(香港)有限公司",
                    NameHK = "250 -花旗銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "251 -ICICI Bank Limited",
                    NameCN = "251 -ICICI Bank Limited",
                    NameHK = "251 -ICICI Bank Limited"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "254 -Melli Bank plc",
                    NameCN = "254 -Melli Bank plc",
                    NameHK = "254 -Melli Bank plc"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "256 -Allahabad Bank",
                    NameCN = "256 -阿拉哈巴德銀行",
                    NameHK = "256 -阿拉哈巴德銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "258 -East West Bank",
                    NameCN = "258 -華美銀行",
                    NameHK = "258 -華美銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "259 -Bank of Baroda",
                    NameCN = "259 -巴魯達銀行",
                    NameHK = "259 -巴魯達銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "260 -Far Eastern International Bank",
                    NameCN = "260 -逺東國際商業銀行股份有限公司",
                    NameHK = "260 -逺東國際商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "261 -Axis Bank Limited",
                    NameCN = "261 -Axis Bank Limited",
                    NameHK = "261 -Axis Bank Limited"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "262 -Canara Bank",
                    NameCN = "262 -Canara Bank",
                    NameHK = "262 -Canara Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "263 -Cathay Bank",
                    NameCN = "263 -國泰銀行",
                    NameHK = "263 -國泰銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "264 -Land Bank of Taiwan Co., Ltd.",
                    NameCN = "264 -台灣土地銀行股份有限公司",
                    NameHK = "264 -台灣土地銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "265 -Taiwan Cooperative Bank, Ltd.",
                    NameCN = "265 -合作金庫銀行",
                    NameHK = "265 -合作金庫銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "266 -Punjab National Bank",
                    NameCN = "266 -Punjab National Bank",
                    NameHK = "266 -Punjab National Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "267 -Banco Santander S.A.",
                    NameCN = "267 -西班牙桑坦德銀行有限公司",
                    NameHK = "267 -西班牙桑坦德銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "268 -Union Bank of India",
                    NameCN = "268 -Union Bank of India",
                    NameHK = "268 -Union Bank of India"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "269 -The Shanghai Commercial & Savings Bank, Ltd.",
                    NameCN = "269 -上海商業儲蓄銀行股份有限公司",
                    NameHK = "269 -上海商業儲蓄銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "271 -Industrial Bank of Korea",
                    NameCN = "271 -Industrial Bank of Korea",
                    NameHK = "271 -Industrial Bank of Korea"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "272 -Bank of Singapore Limited",
                    NameCN = "272 -新加坡銀行有限公司",
                    NameHK = "272 -新加坡銀行有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "273 -Shinhan Bank",
                    NameCN = "273 -Shinhan Bank",
                    NameHK = "273 -Shinhan Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "274 -O-Bank Co., Ltd.",
                    NameCN = "274 -王道商業銀行股份有限公司",
                    NameHK = "274 -王道商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "275 -BNP Paribas Securities Services",
                    NameCN = "275 -BNP Paribas Securities Services",
                    NameHK = "275 -BNP Paribas Securities Services"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "276 -China Development Bank",
                    NameCN = "276 -國家開發銀行",
                    NameHK = "276 -國家開發銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "277 -First Abu Dhabi Bank PJSC",
                    NameCN = "277 -First Abu Dhabi Bank PJSC",
                    NameHK = "277 -First Abu Dhabi Bank PJSC"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "278 -Bank J. Safra Sarasin Ltd.",
                    NameCN = "278 -Bank J. Safra Sarasin Ltd",
                    NameHK = "278 -Bank J. Safra Sarasin Ltd"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "307 -ABN AMRO Bank N.V.",
                    NameCN = "307 -ABN AMRO Bank N.V.",
                    NameHK = "307 -ABN AMRO Bank N.V."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "308 -HDFC Bank Limited",
                    NameCN = "308 -HDFC Bank Limited",
                    NameHK = "308 -HDFC Bank Limited"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "309 -Union Bancaire Privee, UBP SA",
                    NameCN = "309 -Union Bancaire Privee, UBP SA",
                    NameHK = "309 -Union Bancaire Privee, UBP SA"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "316 -Skandinaviska Enskilda Banken AB",
                    NameCN = "316 -Skandinaviska Enskilda Banken AB",
                    NameHK = "316 -Skandinaviska Enskilda Banken AB"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "320 -Bank Julius Baer & Co. Ltd.",
                    NameCN = "320 -Bank Julius Baer & Co. Ltd.",
                    NameHK = "320 -Bank Julius Baer & Co. Ltd."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "324 -Credit Industriel et Commercial",
                    NameCN = "324 -Credit Industriel et Commercial",
                    NameHK = "324 -Credit Industriel et Commercial"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "337 -Taiwan Shin Kong Commercial Bank Co., Ltd.",
                    NameCN = "337 -臺灣新光商業銀行股份有限公司",
                    NameHK = "337 -臺灣新光商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "338 -Bank of China Limited, Hong Kong Branch",
                    NameCN = "338 -中國銀行香港分行",
                    NameHK = "338 -中國銀行香港分行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "339 -CA Indosuez (Switzerland) SA",
                    NameCN = "339 -CA Indosuez (Switzerland) SA",
                    NameHK = "339 -CA Indosuez (Switzerland) SA"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "341 -ICBC Standard Bank PLC",
                    NameCN = "341 -ICBC Standard Bank PLC",
                    NameHK = "341 -ICBC Standard Bank PLC"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "342 -LGT Bank AG",
                    NameCN = "342 -LGT Bank AG",
                    NameHK = "342 -LGT Bank AG"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "344 -Macquarie Bank Limited",
                    NameCN = "344 -Macquarie Bank Limited",
                    NameHK = "344 -Macquarie Bank Limited"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "345 -Shanghai Pudong Development Bank Co., Ltd.",
                    NameCN = "345 -上海浦東發展銀行股份有限公司",
                    NameHK = "345 -上海浦東發展銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "353 -China Minsheng Banking Corp., Ltd.",
                    NameCN = "353 -中國民生銀行股份有限公司",
                    NameHK = "353 -中國民生銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "357 -Pictet & Cie (Europe) S.A.",
                    NameCN = "357 -Pictet & Cie (Europe) S.A.",
                    NameHK = "357 -Pictet & Cie (Europe) S.A."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "360 -The Royal Bank of Scotland N.V.",
                    NameCN = "360 -The Royal Bank of Scotland N.V.",
                    NameHK = "360 -The Royal Bank of Scotland N.V."
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "368 -China Everbright Bank Co., Ltd.",
                    NameCN = "368 -中國光大銀行股份有限公司",
                    NameHK = "368 -中國光大銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "371 -Sumitomo Mitsui Trust Bank, Limited",
                    NameCN = "371 -三井住友信託銀行",
                    NameHK = "371 -三井住友信託銀行"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "372 -Bank of Shanghai (Hong Kong) Limited",
                    NameCN = "372 -上海銀行(香港)有限公司",
                    NameHK = "372 -上海銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "374 -CIMB Bank Berhad",
                    NameCN = "374 -CIMB Bank Berhad",
                    NameHK = "374 -CIMB Bank Berhad"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "377 -Industrial Bank Co., Ltd.",
                    NameCN = "377 -興業銀行股份有限公司",
                    NameHK = "377 -興業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "378 -Yuanta Commercial Bank Co., Ltd.",
                    NameCN = "378 -元大商業銀行股份有限公司",
                    NameHK = "378 -元大商業銀行股份有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "379 -Mashreq Bank - Public Shareholding Company",
                    NameCN = "379 -Mashreq Bank - Public Shareholding Company",
                    NameHK = "379 -Mashreq Bank - Public Shareholding Company"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "381 -Kookmin Bank",
                    NameCN = "381 -Kookmin Bank",
                    NameHK = "381 -Kookmin Bank"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "382 -Bank of Communications (Hong Kong) Limited",
                    NameCN = "382 -交通銀行(香港)有限公司",
                    NameHK = "382 -交通銀行(香港)有限公司"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "868 -CLS Bank International",
                    NameCN = "868 -CLS Bank International",
                    NameHK = "868 -CLS Bank International"
                });


            }
        }
        private void CreateTransactionType(ApplicationDbContext context)
        {
            if (!context.TransactionTypes.Any())
            {
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Waive",
                    NameCN = "Waive",
                    NameHK = "Waive",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Debit Note",
                    NameCN = "Debit Note",
                    NameHK = "Debit Note",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Cheque",
                    NameCN = "Cheque",
                    NameHK = "Cheque",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Other payment method",
                    NameCN = "Other payment method",
                    NameHK = "Other payment method",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Credit Card",
                    NameCN = "Credit Card",
                    NameHK = "Credit Card",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "7-11 Convinience Store",
                    NameCN = "7-11 Convinience Store",
                    NameHK = "7-11 Convinience Store",
                });
                context.TransactionTypes.Add(new TransactionType()
                {
                    NameEN = "Refund",
                    NameCN = "Refund",
                    NameHK = "Refund",
                });
            }
        }

        private void CreateInvoiceItemType(ApplicationDbContext context)
        {
            if (!context.InvoiceItemTypes.Any())
            {
                context.InvoiceItemTypes.Add(new InvoiceItemType()
                {
                    Name = "Course Fee"
                });
                context.InvoiceItemTypes.Add(new InvoiceItemType()
                {
                    Name = "Re-exam Fee"
                });
                context.InvoiceItemTypes.Add(new InvoiceItemType()
                {
                    Name = "Discount"
                });
                context.InvoiceItemTypes.Add(new InvoiceItemType()
                {
                    Name = "Others"
                });
            }
        }

        private void CreateInvoiceStatus(ApplicationDbContext context)
        {
            if (!context.InvoiceStatus.Any())
            {
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Created"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Offered"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Paid Partially"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Waived"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Settled"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Revised"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Overpaid"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Cancelled"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Refund Pending for Approval"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Pending For Refund"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Refunded"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Overdue"
                });
                context.InvoiceStatus.Add(new InvoiceStatus()
                {
                    Name = "Settled (by Batch)"
                });
            }
        }

        private void CreateClients(ApplicationDbContext context)
        {
            if (!context.ClientApplications.Any())
            {
                context.ClientApplications.Add(new ClientApplication()
                {
                    ClientId = "c893c95a6a7d9a42faacfcb69404488695902b7ffcf6441f1b27851778e7b3f5",
                    ClientName = "AdminPortal",
                    ClientSecret = "",
                    ClientUrl = "https://spdc-admin.ntq.solutions/home"
                });

                context.ClientApplications.Add(new ClientApplication()
                {
                    ClientId = "c303efe6964e5c52402be80684ae51028ba12ef512dfd29cdccb47c0c984ce3e",
                    ClientName = "ApplicantPortal",
                    ClientSecret = "",
                    ClientUrl = "http://spdc.ntq.solutions/"
                });
            }
        }

        private void CreateMediumOfInstructions(ApplicationDbContext context)
        {
            if (!context.MediumOfInstructions.Any())
            {
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 1,
                    NameEN = "Cantonese",
                    NameCN = "廣東話",
                    NameHK = "广东话"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 2,
                    NameEN = "Cantonese supported with English handouts",
                    NameCN = "廣東話，附以英文講義",
                    NameHK = "广东话，附以英文讲义"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 3,
                    NameEN = "English only",
                    NameCN = "英文",
                    NameHK = "英文"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 4,
                    NameEN = "Chinese or English",
                    NameCN = "中文 或 英文",
                    NameHK = "中文 或 英文"
                });

                context.SaveChanges();
            }
        }

        private void CreateProgrammeLeaders(ApplicationDbContext context)
        {
            if (!context.ProgrammeLeaders.Any())
            {
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 1,
                    NameEN = "Fred CHAN",
                    NameCN = "黃德偉",
                    NameHK = "黄德伟"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 2,
                    NameEN = "Terence WONG",
                    NameCN = "陳恩輝",
                    NameHK = "陈恩辉"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 3,
                    NameEN = "Stephen LEE",
                    NameCN = "李偉明",
                    NameHK = "李伟明"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 4,
                    NameEN = "Paul YUNG",
                    NameCN = "翁強盛",
                    NameHK = "翁强盛"
                });

                context.SaveChanges();
            }
        }

        private void CreateLectures(ApplicationDbContext context)
        {
            if (!context.Lecturers.Any())
            {
                context.Lecturers.Add(new Lecturer
                {
                    Id = 1,
                    NameEN = "Professionals",
                    NameCN = "專業人士",
                    NameHK = "专业人士"
                });
                context.Lecturers.Add(new Lecturer
                {
                    Id = 2,
                    NameEN = "HKIC Lecturers",
                    NameCN = "香港建造學院講師",
                    NameHK = "香港建造学院讲师"
                });
                context.Lecturers.Add(new Lecturer
                {
                    Id = 3,
                    NameEN = "Professionals / HKIC Lecturers",
                    NameCN = "專業人士 / 香港建造學院講師",
                    NameHK = "专业人士 / 香港建造学院讲师"
                });
                context.SaveChanges();
            }
        }

        private void CreateCmsContentTypes(ApplicationDbContext context)
        {
            if (!context.CmsContentTypes.Any())
            {
                // Banner & Background Image
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Background Image - EN/TC/SC",
                        NameTC = "背景圖片 - EN/TC/SC",
                        NameSC = "背景圖片 - EN/TC/SC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Side Navigation Image - EN/TC/SC",
                        NameTC = "側面導航圖像 - EN/TC/SC",
                        NameSC = "側面導航圖像 - EN/TC/SC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - EN",
                        NameTC = "登陸頁面 - 橫幅 - EN",
                        NameSC = "登陸頁面 - 橫幅 - EN",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - SC",
                        NameTC = "登陸頁面 - 橫幅 - TC",
                        NameSC = "登陸頁面 - 橫幅 - TC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - TC",
                        NameTC = "登陸頁面 - 橫幅 - SC",
                        NameSC = "登陸頁面 - 橫幅 - SC",
                        CmsType = "Banner & Background Image"
                    });

                // Announcement
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - EN",
                        NameTC = "通告 - EN",
                        NameSC = "通告 - EN",
                        CmsType = "Announcement"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - TC",
                        NameTC = "通告 - TC",
                        NameSC = "通告 - TC",
                        CmsType = "Announcement"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - SC",
                        NameTC = "通告 - SC",
                        NameSC = "通告 - SC",
                        CmsType = "Announcement"
                    });

                // News and Events
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - EN",
                        NameTC = "最新消息 - EN",
                        NameSC = "最新消息 - EN",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - TC",
                        NameTC = "最新消息 - TC",
                        NameSC = "最新消息 - TC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - SC",
                        NameTC = "最新消息 - SC",
                        NameSC = "最新消息 - SC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - EN",
                        NameTC = "活動 - EN",
                        NameSC = "活動 - EN",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - TC",
                        NameTC = "活動 - TC",
                        NameSC = "活動 - TC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - SC",
                        NameTC = "活動 - SC",
                        NameSC = "活動 - SC",
                        CmsType = "News and Events"
                    });

                // Promotional Items
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 4 - EN",
                        NameTC = "CMS 4 - EN",
                        NameSC = "CMS 4 - EN",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 4 - TC",
                        NameTC = "CMS 4 - TC",
                        NameSC = "CMS 4 - TC",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 4 - SC",
                        NameTC = "CMS 4 - SC",
                        NameSC = "CMS 4 - SC",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 5  - EN",
                        NameTC = "CMS 5  - EN",
                        NameSC = "CMS 5  - EN",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 5  - TC",
                        NameTC = "CMS 5  - TC",
                        NameSC = "CMS 5  - TC",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 5  - SC",
                        NameTC = "CMS 5  - SC",
                        NameSC = "CMS 5  - SC",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 6 - EN",
                        NameTC = "CMS 6 - EN",
                        NameSC = "CMS 6 - EN",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 6 - TC",
                        NameTC = "CMS 6 - TC",
                        NameSC = "CMS 6 - TC",
                        CmsType = "Promotional Items"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "CMS 6 - SC",
                        NameTC = "CMS 6 - SC",
                        NameSC = "CMS 6 - SC",
                        CmsType = "Promotional Items"
                    });

                // Other Inner Pages
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Disclaimer - EN",
                        NameTC = "免責聲明 - EN",
                        NameSC = "免責聲明 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Disclaimer - TC",
                        NameTC = "免責聲明 - TC",
                        NameSC = "免責聲明 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Disclaimer - SC",
                        NameTC = "免責聲明 - SC",
                        NameSC = "免責聲明 - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - EN",
                        NameTC = "私隱政策 - EN",
                        NameSC = "私隱政策 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - TC",
                        NameTC = "私隱政策 - TC",
                        NameSC = "私隱政策 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - SC",
                        NameTC = "私隱政策 - SC",
                        NameSC = "私隱政策 - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "FAQ - EN",
                        NameTC = "FAQ - EN",
                        NameSC = "FAQ - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "FAQ - TC",
                        NameTC = "FAQ - TC",
                        NameSC = "FAQ - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "FAQ - SC",
                        NameTC = "FAQ - SC",
                        NameSC = "FAQ - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Campus Information - EN",
                        NameTC = "校園資訊 - EN",
                        NameSC = "校園資訊 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Campus Information - TC",
                        NameTC = "校園資訊 - TC",
                        NameSC = "校園資訊 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Campus Information - SC",
                        NameTC = "校園資訊 - SC",
                        NameSC = "校園資訊 - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - EN",
                        NameTC = "有用連結 - EN",
                        NameSC = "有用連結 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - TC",
                        NameTC = "有用連結 - TC",
                        NameSC = "有用連結 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - SC",
                        NameTC = "有用連結 - SC",
                        NameSC = "有用連結 - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - EN",
                        NameTC = "關於我們 - EN",
                        NameSC = "關於我們 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - TC",
                        NameTC = "關於我們 - TC",
                        NameSC = "關於我們 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - SC",
                        NameTC = "關於我們 - SC",
                        NameSC = "關於我們 - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - EN",
                        NameTC = "聯絡我們 - EN",
                        NameSC = "聯絡我們 - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - TC",
                        NameTC = "聯絡我們 - TC",
                        NameSC = "聯絡我們 - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - SC",
                        NameTC = "聯絡我們 - SC",
                        NameSC = "聯絡我們 - SC",
                        CmsType = "Other Inner Pages"
                    });

                context.SaveChanges();
            }
        }

        private void CreateCourseLocations(ApplicationDbContext context)
        {
            if (!context.CourseLocations.Any())
            {
                context.CourseLocations.Add(
                    new CourseLocation
                    {
                        Status = 1,
                        CourseLocationTrans = {
                            new CourseLocationTran(){
                                LanguageId = 1,
                                Name = "Kowloon Bay Campus, HKIC, 44 Tai Yip Street, Kowloon Bay, Kowloon, Hong Kong",
                                Desciption = "Kowloon Bay Campus, HKIC, 44 Tai Yip Street, Kowloon Bay, Kowloon, Hong Kong"
                            },
                            new CourseLocationTran(){
                                LanguageId = 2,
                                Name = "香港九龙九龙湾大业街44号香港建造学院九龙湾院校",
                                Desciption = "Kowloon Bay Campus, HKIC, 44 Tai Yip Street, Kowloon Bay, Kowloon, Hong Kong"
                            },
                            new CourseLocationTran(){
                                LanguageId = 3,
                                Name = "香港九龍九龍灣大業街44號香港建造學院九龍灣院校",
                                Desciption = "Kowloon Bay Campus, HKIC, 44 Tai Yip Street, Kowloon Bay, Kowloon, Hong Kong"
                            } }
                    });
                context.SaveChanges();
            }
        }

        private void CreateLevelofApprovals(ApplicationDbContext context)
        {
            if (!context.LevelofApprovals.Any())
            {
                context.LevelofApprovals.Add(new LevelofApproval
                {
                    Id = 1,
                    NameEN = "1 Level",
                    NameCN = "1級",
                    NameHK = "1級"
                });

                context.LevelofApprovals.Add(new LevelofApproval
                {
                    Id = 2,
                    NameEN = "2 Levels",
                    NameCN = "2級",
                    NameHK = "2級"
                });
                context.SaveChanges();
            }
        }

        private void CreateCourseCategories(ApplicationDbContext context)
        {
            if (!context.CourseCategories.Any())
            {
                var initCategory1 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory1.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Building Information Modelling(BIM)", Title = "Building Information Modelling(BIM)" });
                initCategory1.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "建築信息模擬(BIM)", Title = "建築信息模擬(BIM)" });
                initCategory1.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "建筑信息模拟(BIM)", Title = "建筑信息模拟(BIM)" });
                context.CourseCategories.Add(initCategory1);

                var initCategory2 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Modular Integrated Construction(MiC)", Title = "Modular Integrated Construction(MiC)" });
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "組裝合成建築法(MiC)", Title = "組裝合成建築法(MiC)" });
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "组装合成建筑法(MiC)", Title = "组装合成建筑法(MiC)" });
                context.CourseCategories.Add(initCategory2);

                var initCategory3 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Courses Recognised by Government Departments", Title = "Courses Recognised by Government Departments" });
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "政府部門認可課程", Title = "政府部門認可課程" });
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "政府部门认可课程", Title = "政府部门认可课程" });
                context.CourseCategories.Add(initCategory3);

                var initCategory4 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Courses Recognised in Public Work Contracts", Title = "Courses Recognised in Public Work Contracts" });
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "公共工程合約認可課程", Title = "公共工程合約認可課程" });
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "公共工程合约认可课程", Title = "公共工程合约认可课程" });
                context.CourseCategories.Add(initCategory4);

                var initCategory5 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "AutoCAD Drafting", Title = "AutoCAD Drafting" });
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "電腦輔助繪圖", Title = "電腦輔助繪圖" });
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "电脑辅助绘图", Title = "电脑辅助绘图" });
                context.CourseCategories.Add(initCategory5);

                var initCategory6 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Contract Administration", Title = "Contract Administration" });
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "合約管理", Title = "合約管理" });
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "合约管理", Title = "合约管理" });
                context.CourseCategories.Add(initCategory6);

                var initCategory7 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Construction Technology and Quality", Title = "Construction Technology and Quality" });
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "建造技術及品質", Title = "建造技術及品質" });
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "建造技术及品质", Title = "建造技术及品质" });
                context.CourseCategories.Add(initCategory7);

                var initCategory8 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Construction Management", Title = "Construction Management" });
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "建造管理", Title = "建造管理" });
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "建造管理", Title = "建造管理" });
                context.CourseCategories.Add(initCategory8);

                var initCategory9 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Environmental Protection", Title = "Environmental Protection" });
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "環境保護", Title = "環境保護" });
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "环境保护", Title = "环境保护" });
                context.CourseCategories.Add(initCategory9);

                var initCategory10 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Conservation of Built Heritage", Title = "Conservation of Built Heritage" });
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "文物保育", Title = "文物保育" });
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "文物保育", Title = "文物保育" });
                context.CourseCategories.Add(initCategory10);
            }
        }

        private void CreateCourseType(ApplicationDbContext context)
        {
            if (!context.CourseTypes.Any())
            {
                context.CourseTypes.Add(new CourseType() { NameEN = "Part Time", NameHK = "兼讀制", NameCN = "兼读制" });
                context.CourseTypes.Add(new CourseType() { NameEN = "Full Time", NameHK = "全日制", NameCN = "全日制" });
            }
        }

        private void CreateUser(ApplicationDbContext context)
        {
            var manager = new ApplicationUserManager(new CustomUserStore(new ApplicationDbContext()));
            var roleManager = new ApplicationRoleManager(new CustomRoleStore(new ApplicationDbContext()));

            //var superadmin = new ApplicationUser()
            //{
            //    UserName = "superadmin",
            //    Email = "superadmin@gmail.com",
            //    EmailConfirmed = true,
            //    CICNumber = "superadmin"
            //};
            //manager.Create(superadmin, "123456aA@");

            var admin = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                CICNumber = "admin"
            };
            manager.Create(admin, "123456aA@");

            //var user = new ApplicationUser()
            //{
            //    UserName = "user",
            //    Email = "user@gmail.com",
            //    EmailConfirmed = true,
            //    CICNumber = "user"
            //};
            //manager.Create(user, "123456aA@");

            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new CustomRole { Name = "SuperAdmin" });
            //    roleManager.Create(new CustomRole { Name = "Admin" });
            //    roleManager.Create(new CustomRole { Name = "User" });
            //}

            //var superadminUser = manager.FindByEmail("superadmin@gmail.com");
            var adminUser = manager.FindByEmail("admin@gmail.com");
            //var normalUser = manager.FindByEmail("user@gmail.com");

            //manager.AddToRoles(superadminUser.Id, new string[] { "SuperAdmin" });
            manager.AddToRoles(adminUser.Id, new string[] { "Admin" });
            //manager.AddToRoles(normalUser.Id, new string[] { "Admin" });
        }

        private void CreateLanguage(ApplicationDbContext context)
        {
            if (!context.Languages.Any())
            {
                context.Languages.Add(new Language { Name = "EN", Code = "EN" });
                context.Languages.Add(new Language { Name = "简", Code = "CN" });
                context.Languages.Add(new Language { Name = "繁", Code = "HK" });
                context.SaveChanges();
            }
        }

        private void CreateClassCommon(ApplicationDbContext context)
        {
            if (!context.ClassCommon.Any())
            {
                context.ClassCommon.Add(new ClassCommon() { Id = 1, TypeName = "%", TypeCommon = 1 });
                context.ClassCommon.Add(new ClassCommon() { Id = 2, TypeName = "Lesson(s)", TypeCommon = 1 });
                context.ClassCommon.Add(new ClassCommon() { Id = 3, TypeName = "Hrs", TypeCommon = 1 });

                var date = DateTime.Now.Year;
                var countYear = 0;
                int i = 4;
                for (; i < 100; i++)
                {
                    var strYear = (date + countYear).ToString();
                    context.ClassCommon.Add(new ClassCommon() { Id = i, TypeName = strYear, TypeCommon = 2 });
                    countYear += 1;
                    if (strYear.Substring(2).Equals("99"))
                        break;
                }

                context.ClassCommon.Add(new ClassCommon() { Id = i++, TypeName = "Marks", TypeCommon = 3 });
                context.ClassCommon.Add(new ClassCommon() { Id = i++, TypeName = "%", TypeCommon = 3 });
                context.SaveChanges();
            }
        }

        private void CreateEnrnollmentStatus(ApplicationDbContext context)
        {
            if (!context.EnrollmentStatus.Any())
            {
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Enrolled", NameTC = "Enrolled - TC", NameSC = "Enrolled - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Course Completed", NameTC = "Course Completed - TC", NameSC = "Course Completed - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Graduated", NameTC = "Graduated - TC", NameSC = "Graduated - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Resit", NameTC = "Resit - TC", NameSC = "Resit - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Withdrawal", NameTC = "Withdrawal - TC", NameSC = "Withdrawal - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Failed", NameTC = "Failed - TC", NameSC = "Failed - SC" });
                context.EnrollmentStatus.Add(new EnrollmentStatus() { NameEN = "Appeal", NameTC = "Appeal - TC", NameSC = "Appeal - SC" });
            }
        }

    }
}

