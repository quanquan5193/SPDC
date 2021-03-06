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
                    NameCN = "003 -????????????(??????)????????????",
                    NameHK = "003 -????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "004 -The Hongkong and Shanghai Banking Corporation Limited",
                    NameCN = "004 -????????????????????????????????????",
                    NameHK = "004 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "005 -Credit Agricole Corporate and Investment Bank",
                    NameCN = "005 -??????????????????",
                    NameHK = "005 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "006 -Citibank, N.A.",
                    NameCN = "006 -????????????",
                    NameHK = "006 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "007 -JPMorgan Chase Bank, N.A.",
                    NameCN = "007 -??????????????????",
                    NameHK = "007 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "008 -The Royal Bank of Scotland plc",
                    NameCN = "008 -?????????????????????????????????",
                    NameHK = "008 -?????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "009 -China Construction Bank (Asia) Corporation Limited",
                    NameCN = "009 -??????????????????(??????)??????????????????",
                    NameHK = "009 -??????????????????(??????)??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "012 -Bank of China (Hong Kong) Limited",
                    NameCN = "012 -????????????(??????)????????????",
                    NameHK = "012 -????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "015 -The Bank of East Asia, Limited",
                    NameCN = "015 -????????????????????????",
                    NameHK = "015 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "016 -DBS Bank (Hong Kong) Limited",
                    NameCN = "016 -????????????(??????)????????????",
                    NameHK = "016 -????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "018 -China CITIC Bank International Limited",
                    NameCN = "018 -??????????????????????????????",
                    NameHK = "018 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "020 -Wing Lung Bank Ltd.",
                    NameCN = "020 -????????????????????????",
                    NameHK = "020 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "022 -Oversea-Chinese Banking Corporation Ltd.",
                    NameCN = "022 -????????????",
                    NameHK = "022 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "024 -Hang Seng Bank Ltd.",
                    NameCN = "024 -????????????????????????",
                    NameHK = "024 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "025 -Shanghai Commercial Bank Ltd.",
                    NameCN = "025 -??????????????????????????????",
                    NameHK = "025 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "027 -Bank of Communications Co., Ltd.",
                    NameCN = "027 -??????????????????????????????",
                    NameHK = "027 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "028 -Public Bank (Hong Kong) Limited",
                    NameCN = "028 -????????????(??????)????????????",
                    NameHK = "028 -????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "035 -OCBC Wing Hang Bank Limited",
                    NameCN = "035 -??????????????????????????????",
                    NameHK = "035 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "038 -Tai Yau Bank Ltd.",
                    NameCN = "038 -????????????????????????",
                    NameHK = "038 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "039 -Chiyu Banking Corporation Ltd.",
                    NameCN = "039 -????????????????????????",
                    NameHK = "039 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "040 -Dah Sing Bank, Ltd.",
                    NameCN = "040 -????????????????????????",
                    NameHK = "040 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "041 -Chong Hing Bank Limited",
                    NameCN = "041 -????????????????????????",
                    NameHK = "041 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "043 -Nanyang Commercial Bank, Ltd.",
                    NameCN = "043 -??????????????????????????????",
                    NameHK = "043 -??????????????????????????????"
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
                    NameCN = "047 -???????????? UFJ ??????",
                    NameHK = "047 -???????????? UFJ ??????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "049 -Bangkok Bank Public Company Limited",
                    NameCN = "049 -????????????",
                    NameHK = "049 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "050 -Indian Overseas Bank",
                    NameCN = "050 -??????????????????",
                    NameHK = "050 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "054 -Deutsche Bank AG",
                    NameCN = "054 -???????????????",
                    NameHK = "054 -???????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "055 -Bank of America, N.A.",
                    NameCN = "055 -????????????",
                    NameHK = "055 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "056 -BNP Paribas",
                    NameCN = "056 -??????????????????",
                    NameHK = "056 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "058 -Bank of India",
                    NameCN = "058 -????????????",
                    NameHK = "058 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "060 -National Bank of Pakistan",
                    NameCN = "060 -????????????????????????",
                    NameHK = "060 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "061 -Tai Sang Bank Limited",
                    NameCN = "061 -????????????????????????",
                    NameHK = "061 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "063 -Malayan Banking Berhad (Maybank)",
                    NameCN = "063 -???????????????",
                    NameHK = "063 -???????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "065 -Sumitomo Mitsui Banking Corporation",
                    NameCN = "065 -??????????????????",
                    NameHK = "065 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "066 -PT. Bank Negara Indonesia (Persero) Tbk.",
                    NameCN = "066 -??????????????????",
                    NameHK = "066 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "067 -BDO Unibank, Inc.",
                    NameCN = "067 -????????????????????????",
                    NameHK = "067 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "071 -United Overseas Bank Ltd.",
                    NameCN = "071 -????????????????????????",
                    NameHK = "071 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "072 -Industrial and Commercial Bank of China (Asia) Limited",
                    NameCN = "072 -??????????????????(??????)????????????",
                    NameHK = "072 -??????????????????(??????)????????????"
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
                    NameCN = "076 -?????????????????????",
                    NameHK = "076 -?????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "080 -Royal Bank of Canada",
                    NameCN = "080 -?????????????????????",
                    NameHK = "080 -?????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "081 -Societe Generale",
                    NameCN = "081 -??????????????????",
                    NameHK = "081 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "082 -State Bank of India",
                    NameCN = "082 -??????????????????",
                    NameHK = "082 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "085 -The Toronto-Dominion Bank",
                    NameCN = "085 -??????????????????????????????",
                    NameHK = "085 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "086 -Bank of Montreal",
                    NameCN = "086 -???????????????",
                    NameHK = "086 -???????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "092 -Canadian Imperial Bank of Commerce",
                    NameCN = "092 -???????????????????????????",
                    NameHK = "092 -???????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "097 -Commerzbank AG",
                    NameCN = "097 -??????????????????",
                    NameHK = "097 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "103 -UBS AG, Hong Kong",
                    NameCN = "103 -????????????",
                    NameHK = "103 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "106 -HSBC Bank USA, N.A.",
                    NameCN = "106 -??????????????????",
                    NameHK = "106 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "109 -Mizuho Bank, Ltd., Hong Kong Branch",
                    NameCN = "109 -????????????",
                    NameHK = "109 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "113 -DZ BANK AG Deutsche Zentral-Genossenschaftsbank",
                    NameCN = "113 -????????????????????????",
                    NameHK = "113 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "118 -Woori Bank",
                    NameCN = "118 -????????????",
                    NameHK = "118 -????????????"
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
                    NameCN = "128 -????????????(??????)????????????",
                    NameHK = "128 -????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "138 -Mitsubishi UFJ Trust and Banking Corporation",
                    NameCN = "138 -?????? UFJ ????????????",
                    NameHK = "138 -?????? UFJ ????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "139 -The Bank of New York Mellon",
                    NameCN = "139 -??????????????????????????????",
                    NameHK = "139 -??????????????????????????????"
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
                    NameCN = "147 -?????????????????????",
                    NameHK = "147 -?????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "150 -National Australia Bank Limited",
                    NameCN = "150 -????????????",
                    NameHK = "150 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "151 -Westpac Banking Corporation",
                    NameCN = "151 -??????????????????",
                    NameHK = "151 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "152 -Australia and New Zealand Banking Group Ltd.",
                    NameCN = "152 -???????????????????????????",
                    NameHK = "152 -???????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "153 -Commonwealth Bank of Australia",
                    NameCN = "153 -??????????????????",
                    NameHK = "153 -??????????????????"
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
                    NameCN = "164 -????????????",
                    NameHK = "164 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "165 -Svenska Handelsbanken AB (publ)",
                    NameCN = "165 -??????????????????",
                    NameHK = "165 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "170 -The Chiba Bank Ltd.",
                    NameCN = "170 -????????????",
                    NameHK = "170 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "178 -KBC Bank N.V., Hong Kong Branch",
                    NameCN = "178 -?????????????????????",
                    NameHK = "178 -?????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "180 -Wells Fargo Bank, N.A., Hong Kong Branch",
                    NameCN = "180 -????????????????????????",
                    NameHK = "180 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "183 -Co??peratieve Rabobank U.A.",
                    NameCN = "183 -??????????????????",
                    NameHK = "183 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "185 -DBS Bank Ltd, Hong Kong Branch",
                    NameCN = "185 -????????????????????????",
                    NameHK = "185 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "186 -The Shizuoka Bank Ltd.",
                    NameCN = "186 -????????????",
                    NameHK = "186 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "188 -The Hachijuni Bank, Ltd.",
                    NameCN = "188 -???????????????",
                    NameHK = "188 -???????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "198 -Hua Nan Commercial Bank, Ltd.",
                    NameCN = "198 -????????????????????????????????????",
                    NameHK = "198 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "199 -The Shiga Bank, Ltd.",
                    NameCN = "199 -????????????",
                    NameHK = "199 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "201 -Bank of Taiwan",
                    NameCN = "201 -??????????????????????????????",
                    NameHK = "201 -??????????????????????????????"
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
                    NameCN = "203 -????????????????????????????????????",
                    NameHK = "203 -????????????????????????????????????"
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
                    NameCN = "206 -????????????????????????????????????",
                    NameHK = "206 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "210 -Natixis",
                    NameCN = "210 -??????????????????",
                    NameHK = "210 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "214 -Industrial and Commercial Bank of China Limited",
                    NameCN = "214 -????????????????????????????????????",
                    NameHK = "214 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "220 -State Street Bank and Trust Company",
                    NameCN = "220 -??????????????????",
                    NameHK = "220 -??????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "221 -China Construction Bank Corporation",
                    NameCN = "221 -????????????????????????????????????",
                    NameHK = "221 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "222 -Agricultural Bank of China Limited",
                    NameCN = "222 -????????????????????????????????????",
                    NameHK = "222 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "224 -The Iyo Bank, Ltd., Hong Kong Branch",
                    NameCN = "224 -????????????",
                    NameHK = "224 -????????????"
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
                    NameCN = "229 -??????????????????????????????????????????",
                    NameHK = "229 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "230 -Taiwan Business Bank, Hong Kong Branch",
                    NameCN = "230 -????????????????????????",
                    NameHK = "230 -????????????????????????"
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
                    NameCN = "234 -????????????????????????????????????",
                    NameHK = "234 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "235 -HSBC Private Bank (Suisse) SA",
                    NameCN = "235 -??????????????????(??????)????????????",
                    NameHK = "235 -??????????????????(??????)????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "236 -Cathay United Bank Company, Limited",
                    NameCN = "236 -??????????????????????????????????????????",
                    NameHK = "236 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "237 -EFG Bank AG",
                    NameCN = "237 -????????????????????????????????????",
                    NameHK = "237 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "238 -China Merchants Bank Co., Ltd.",
                    NameCN = "238 -??????????????????????????????",
                    NameHK = "238 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "239 -Taipei Fubon Commercial Bank Co., Ltd.",
                    NameCN = "239 -??????????????????????????????????????????",
                    NameHK = "239 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "241 -Bank SinoPac",
                    NameCN = "241 -????????????????????????????????????",
                    NameHK = "241 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "242 -Mega International Commercial Bank Co., Ltd.",
                    NameCN = "242 -????????????????????????",
                    NameHK = "242 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "243 -E.Sun Commercial Bank, Ltd.",
                    NameCN = "243 -????????????????????????????????????",
                    NameHK = "243 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "245 -Taishin International Bank Co., Ltd.",
                    NameCN = "245 -??????????????????????????????????????????",
                    NameHK = "245 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "248 -Hong Leong Bank Berhad",
                    NameCN = "248 -????????????????????????",
                    NameHK = "248 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "249 -Standard Chartered Bank Hong Kong Branch",
                    NameCN = "249 -????????????",
                    NameHK = "249 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "250 -Citibank (Hong Kong) Limited",
                    NameCN = "250 -????????????(??????)????????????",
                    NameHK = "250 -????????????(??????)????????????"
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
                    NameCN = "256 -?????????????????????",
                    NameHK = "256 -?????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "258 -East West Bank",
                    NameCN = "258 -????????????",
                    NameHK = "258 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "259 -Bank of Baroda",
                    NameCN = "259 -???????????????",
                    NameHK = "259 -???????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "260 -Far Eastern International Bank",
                    NameCN = "260 -??????????????????????????????????????????",
                    NameHK = "260 -??????????????????????????????????????????"
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
                    NameCN = "263 -????????????",
                    NameHK = "263 -????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "264 -Land Bank of Taiwan Co., Ltd.",
                    NameCN = "264 -????????????????????????????????????",
                    NameHK = "264 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "265 -Taiwan Cooperative Bank, Ltd.",
                    NameCN = "265 -??????????????????",
                    NameHK = "265 -??????????????????"
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
                    NameCN = "267 -????????????????????????????????????",
                    NameHK = "267 -????????????????????????????????????"
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
                    NameCN = "269 -??????????????????????????????????????????",
                    NameHK = "269 -??????????????????????????????????????????"
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
                    NameCN = "272 -???????????????????????????",
                    NameHK = "272 -???????????????????????????"
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
                    NameCN = "274 -????????????????????????????????????",
                    NameHK = "274 -????????????????????????????????????"
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
                    NameCN = "276 -??????????????????",
                    NameHK = "276 -??????????????????"
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
                    NameCN = "337 -??????????????????????????????????????????",
                    NameHK = "337 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "338 -Bank of China Limited, Hong Kong Branch",
                    NameCN = "338 -????????????????????????",
                    NameHK = "338 -????????????????????????"
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
                    NameCN = "345 -??????????????????????????????????????????",
                    NameHK = "345 -??????????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "353 -China Minsheng Banking Corp., Ltd.",
                    NameCN = "353 -????????????????????????????????????",
                    NameHK = "353 -????????????????????????????????????"
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
                    NameCN = "368 -????????????????????????????????????",
                    NameHK = "368 -????????????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "371 -Sumitomo Mitsui Trust Bank, Limited",
                    NameCN = "371 -????????????????????????",
                    NameHK = "371 -????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "372 -Bank of Shanghai (Hong Kong) Limited",
                    NameCN = "372 -????????????(??????)????????????",
                    NameHK = "372 -????????????(??????)????????????"
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
                    NameCN = "377 -??????????????????????????????",
                    NameHK = "377 -??????????????????????????????"
                });
                context.AcceptedBanks.Add(new AcceptedBank()
                {
                    NameEN = "378 -Yuanta Commercial Bank Co., Ltd.",
                    NameCN = "378 -????????????????????????????????????",
                    NameHK = "378 -????????????????????????????????????"
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
                    NameCN = "382 -????????????(??????)????????????",
                    NameHK = "382 -????????????(??????)????????????"
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
                    NameCN = "?????????",
                    NameHK = "?????????"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 2,
                    NameEN = "Cantonese supported with English handouts",
                    NameCN = "??????????????????????????????",
                    NameHK = "??????????????????????????????"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 3,
                    NameEN = "English only",
                    NameCN = "??????",
                    NameHK = "??????"
                });
                context.MediumOfInstructions.Add(new MediumOfInstruction
                {
                    Id = 4,
                    NameEN = "Chinese or English",
                    NameCN = "?????? ??? ??????",
                    NameHK = "?????? ??? ??????"
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
                    NameCN = "?????????",
                    NameHK = "?????????"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 2,
                    NameEN = "Terence WONG",
                    NameCN = "?????????",
                    NameHK = "?????????"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 3,
                    NameEN = "Stephen LEE",
                    NameCN = "?????????",
                    NameHK = "?????????"
                });
                context.ProgrammeLeaders.Add(new ProgrammeLeader
                {
                    Id = 4,
                    NameEN = "Paul YUNG",
                    NameCN = "?????????",
                    NameHK = "?????????"
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
                    NameCN = "????????????",
                    NameHK = "????????????"
                });
                context.Lecturers.Add(new Lecturer
                {
                    Id = 2,
                    NameEN = "HKIC Lecturers",
                    NameCN = "????????????????????????",
                    NameHK = "????????????????????????"
                });
                context.Lecturers.Add(new Lecturer
                {
                    Id = 3,
                    NameEN = "Professionals / HKIC Lecturers",
                    NameCN = "???????????? / ????????????????????????",
                    NameHK = "???????????? / ????????????????????????"
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
                        NameTC = "???????????? - EN/TC/SC",
                        NameSC = "???????????? - EN/TC/SC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Side Navigation Image - EN/TC/SC",
                        NameTC = "?????????????????? - EN/TC/SC",
                        NameSC = "?????????????????? - EN/TC/SC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - EN",
                        NameTC = "???????????? - ?????? - EN",
                        NameSC = "???????????? - ?????? - EN",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - SC",
                        NameTC = "???????????? - ?????? - TC",
                        NameSC = "???????????? - ?????? - TC",
                        CmsType = "Banner & Background Image"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Landing Page - Banner - TC",
                        NameTC = "???????????? - ?????? - SC",
                        NameSC = "???????????? - ?????? - SC",
                        CmsType = "Banner & Background Image"
                    });

                // Announcement
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - EN",
                        NameTC = "?????? - EN",
                        NameSC = "?????? - EN",
                        CmsType = "Announcement"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - TC",
                        NameTC = "?????? - TC",
                        NameSC = "?????? - TC",
                        CmsType = "Announcement"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Announcement - SC",
                        NameTC = "?????? - SC",
                        NameSC = "?????? - SC",
                        CmsType = "Announcement"
                    });

                // News and Events
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - EN",
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "News - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - EN",
                        NameTC = "?????? - EN",
                        NameSC = "?????? - EN",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - TC",
                        NameTC = "?????? - TC",
                        NameSC = "?????? - TC",
                        CmsType = "News and Events"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Events - SC",
                        NameTC = "?????? - SC",
                        NameSC = "?????? - SC",
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
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Disclaimer - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Disclaimer - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - EN",
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Privacy Policy - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
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
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Campus Information - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Campus Information - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - EN",
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Useful Links - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - EN",
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "About Us - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - EN",
                        NameTC = "???????????? - EN",
                        NameSC = "???????????? - EN",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - TC",
                        NameTC = "???????????? - TC",
                        NameSC = "???????????? - TC",
                        CmsType = "Other Inner Pages"
                    });
                context.CmsContentTypes.Add(
                    new CmsContentType
                    {
                        Name = "Contact Us - SC",
                        NameTC = "???????????? - SC",
                        NameSC = "???????????? - SC",
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
                                Name = "??????????????????????????????44????????????????????????????????????",
                                Desciption = "Kowloon Bay Campus, HKIC, 44 Tai Yip Street, Kowloon Bay, Kowloon, Hong Kong"
                            },
                            new CourseLocationTran(){
                                LanguageId = 3,
                                Name = "??????????????????????????????44????????????????????????????????????",
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
                    NameCN = "1???",
                    NameHK = "1???"
                });

                context.LevelofApprovals.Add(new LevelofApproval
                {
                    Id = 2,
                    NameEN = "2 Levels",
                    NameCN = "2???",
                    NameHK = "2???"
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
                initCategory1.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "??????????????????(BIM)", Title = "??????????????????(BIM)" });
                initCategory1.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "??????????????????(BIM)", Title = "??????????????????(BIM)" });
                context.CourseCategories.Add(initCategory1);

                var initCategory2 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Modular Integrated Construction(MiC)", Title = "Modular Integrated Construction(MiC)" });
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "?????????????????????(MiC)", Title = "?????????????????????(MiC)" });
                initCategory2.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "?????????????????????(MiC)", Title = "?????????????????????(MiC)" });
                context.CourseCategories.Add(initCategory2);

                var initCategory3 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Courses Recognised by Government Departments", Title = "Courses Recognised by Government Departments" });
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "????????????????????????", Title = "????????????????????????" });
                initCategory3.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "????????????????????????", Title = "????????????????????????" });
                context.CourseCategories.Add(initCategory3);

                var initCategory4 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Courses Recognised in Public Work Contracts", Title = "Courses Recognised in Public Work Contracts" });
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "??????????????????????????????", Title = "??????????????????????????????" });
                initCategory4.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "??????????????????????????????", Title = "??????????????????????????????" });
                context.CourseCategories.Add(initCategory4);

                var initCategory5 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "AutoCAD Drafting", Title = "AutoCAD Drafting" });
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "??????????????????", Title = "??????????????????" });
                initCategory5.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "??????????????????", Title = "??????????????????" });
                context.CourseCategories.Add(initCategory5);

                var initCategory6 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Contract Administration", Title = "Contract Administration" });
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "????????????", Title = "????????????" });
                initCategory6.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "????????????", Title = "????????????" });
                context.CourseCategories.Add(initCategory6);

                var initCategory7 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Construction Technology and Quality", Title = "Construction Technology and Quality" });
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "?????????????????????", Title = "?????????????????????" });
                initCategory7.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "?????????????????????", Title = "?????????????????????" });
                context.CourseCategories.Add(initCategory7);

                var initCategory8 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Construction Management", Title = "Construction Management" });
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "????????????", Title = "????????????" });
                initCategory8.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "????????????", Title = "????????????" });
                context.CourseCategories.Add(initCategory8);

                var initCategory9 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Environmental Protection", Title = "Environmental Protection" });
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "????????????", Title = "????????????" });
                initCategory9.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "????????????", Title = "????????????" });
                context.CourseCategories.Add(initCategory9);

                var initCategory10 = new CourseCategory()
                {
                    Status = 1,
                    CourseCategorieTrans = new List<CourseCategorieTran>()
                };
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 1, Name = "Conservation of Built Heritage", Title = "Conservation of Built Heritage" });
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 2, Name = "????????????", Title = "????????????" });
                initCategory10.CourseCategorieTrans.Add(new CourseCategorieTran() { LanguageId = 3, Name = "????????????", Title = "????????????" });
                context.CourseCategories.Add(initCategory10);
            }
        }

        private void CreateCourseType(ApplicationDbContext context)
        {
            if (!context.CourseTypes.Any())
            {
                context.CourseTypes.Add(new CourseType() { NameEN = "Part Time", NameHK = "?????????", NameCN = "?????????" });
                context.CourseTypes.Add(new CourseType() { NameEN = "Full Time", NameHK = "?????????", NameCN = "?????????" });
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
                context.Languages.Add(new Language { Name = "???", Code = "CN" });
                context.Languages.Add(new Language { Name = "???", Code = "HK" });
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

