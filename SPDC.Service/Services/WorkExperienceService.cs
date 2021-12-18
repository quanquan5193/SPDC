using ClosedXML.Excel;
using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{
    public interface IWorkExperienceService
    {
        Task<List<WorkExperienceBindingModel>> GetUserWorkExperience(int userId, int langCode);
        Task<List<WorkExperienceBindingModel>> GetUserWorkExperienceTemp(int userId, int langCode);
        Task<bool> UpdateUserWorkExperience(List<WorkExperienceBindingModel> model, int langCode, int userId, int type = 0);
        Task<bool> UpdateUserWorkExperienceTempToTable(List<WorkExperienceBindingModel> model, int langCode, int userId, int type = 0);
        Task<bool> UpdateUserWorkExperienceTemp(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0);
        Task<bool> DeleteUserWorkExperienceTemp(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0);
        Task<ResultModel<FileReturnViewModel>> ExportWorkExperience(int userId, int langCode, int type);
    }
    public class WorkExperienceService : IWorkExperienceService
    {
        IUnitOfWork _unitOfWork;
        IWorkExperienceRepository _workExperienceRepository;
        IWorkExperienceTransRepository _workExperienceTransRepository;
        IWorkExperienceTempRepository _workExperienceTempRepository;
        IWorkExperienceTransTempRepository _workExperienceTransTempRepository;
        public WorkExperienceService(IUnitOfWork unitOfWork, IWorkExperienceRepository workExperienceRepository, IWorkExperienceTransRepository workExperienceTransRepository,
            IWorkExperienceTempRepository workExperienceTempRepository, IWorkExperienceTransTempRepository workExperienceTransTempRepository)
        {
            _unitOfWork = unitOfWork;
            _workExperienceRepository = workExperienceRepository;
            _workExperienceTransRepository = workExperienceTransRepository;
            _workExperienceTempRepository = workExperienceTempRepository;
            _workExperienceTransTempRepository = workExperienceTransTempRepository;
        }

        public async Task<List<WorkExperienceBindingModel>> GetUserWorkExperience(int userId, int langCode)
        {
            var lstWorkBinding = new List<WorkExperienceBindingModel>();
            var listWorkkEx = await _workExperienceRepository.GetMulti(x => x.UserId == userId, new string[] { "WorkExperienceTrans" });
            for (int i = 0; i < listWorkkEx.Count; i++)
            {
                var workBinding = new WorkExperienceBindingModel();
                if (listWorkkEx[i].WorkExperienceTrans.Count() > 0)
                {
                    listWorkkEx[i].ConvertToWorkExperienceBindingModel(workBinding);
                    lstWorkBinding.Add(workBinding);
                }
            }

            return lstWorkBinding;
        }

        public async Task<List<WorkExperienceBindingModel>> GetUserWorkExperienceTemp(int userId, int langCode)
        {
            var lstWorkBinding = new List<WorkExperienceBindingModel>();
            var listWorkkEx = await _workExperienceTempRepository.GetMulti(x => x.UserId == userId, new string[] { "WorkExperienceTempTrans" });
            for (int i = 0; i < listWorkkEx.Count; i++)
            {
                var workBinding = new WorkExperienceBindingModel();
                if (listWorkkEx[i].WorkExperienceTempTrans.Count() > 0)
                {
                    listWorkkEx[i].ConvertToWorkExperienceBindingModelTemp(workBinding);
                    lstWorkBinding.Add(workBinding);
                }
            }

            return lstWorkBinding;
        }

        public async Task<bool> UpdateUserWorkExperience(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0)
        {
            if (lstModel == null) return true;
            try
            {
                var lstWorkEx = type == 0 ? await _workExperienceRepository.GetMulti(n => n.UserId == userId, new string[] { "WorkExperienceTrans" })
                    : await _workExperienceRepository.GetMulti(n => n.UserId == userId && n.ClassifyWorkingExperience == type, new string[] { "WorkExperienceTrans" });

                var lstInsert = lstModel.Where(n => n.Id == 0);
                var lstUpdate = lstWorkEx.Where(n => lstModel.Any(x => x.Id == n.Id));
                var lstDelete = lstWorkEx.Where(n => !lstUpdate.Select(x => x.Id).Contains(n.Id));

                foreach (var item in lstDelete)
                {
                    _workExperienceTransRepository.DeleteMulti(n => n.WorkExperienceId == item.Id);
                    _workExperienceRepository.Delete(item);
                }

                foreach (var item in lstUpdate)
                {
                    _workExperienceRepository.Update(EntityHelpers.ToWorkExperience(lstModel.Single(x => x.Id == item.Id), item, userId, langCode));
                }

                foreach (var item in lstInsert)
                {
                    _workExperienceRepository.Add(EntityHelpers.ToWorkExperience(item, null, userId, langCode));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserWorkExperienceTempToTable(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0)
        {
            if (lstModel == null) return true;
            try
            {
                var lstWorkEx = type == 0 ? await _workExperienceRepository.GetMulti(n => n.UserId == userId, new string[] { "WorkExperienceTrans" })
                    : await _workExperienceRepository.GetMulti(n => n.UserId == userId && n.ClassifyWorkingExperience == type, new string[] { "WorkExperienceTrans" });

                var lstInsert = lstModel;
                var lstDelete = lstWorkEx;

                foreach (var item in lstDelete)
                {
                    _workExperienceTransRepository.DeleteMulti(n => n.WorkExperienceId == item.Id);
                    _workExperienceRepository.Delete(item);
                }
                _unitOfWork.Commit();
                foreach (var item in lstInsert)
                {
                    _workExperienceRepository.Add(EntityHelpers.ToWorkExperience(item, null, userId, langCode));
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> DeleteUserWorkExperienceTemp(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0)
        {
            if (lstModel == null) return true;
            try
            {
                var lstWorkEx = type == 0 ? await _workExperienceTempRepository.GetMulti(n => n.UserId == userId, new string[] { "WorkExperienceTempTrans" })
                     : await _workExperienceTempRepository.GetMulti(n => n.UserId == userId && n.ClassifyWorkingExperience == type, new string[] { "WorkExperienceTempTrans" });

                var lstInsert = lstModel;
                var lstDelete = lstWorkEx;

                foreach (var item in lstDelete)
                {
                    _workExperienceTransTempRepository.DeleteMulti(n => n.WorkExperienceTemp.Id == item.Id);
                    _workExperienceTempRepository.Delete(item);
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateUserWorkExperienceTemp(List<WorkExperienceBindingModel> lstModel, int langCode, int userId, int type = 0)
        {
            if (lstModel == null) return true;
            try
            {
                var lstWorkEx = type == 0 ? await _workExperienceTempRepository.GetMulti(n => n.UserId == userId, new string[] { "WorkExperienceTempTrans" })
                    : await _workExperienceTempRepository.GetMulti(n => n.UserId == userId && n.ClassifyWorkingExperience == type, new string[] { "WorkExperienceTempTrans" });

                var lstInsert = lstModel;
                var lstDelete = lstWorkEx;

                foreach (var item in lstDelete)
                {
                    _workExperienceTransTempRepository.DeleteMulti(n => n.WorkExperienceTemp.Id == item.Id);
                    _workExperienceTempRepository.Delete(item);
                }
                foreach (var item in lstInsert)
                {
                    _workExperienceTempRepository.Add(EntityHelpers.ToWorkExperienceTemp(item, null, userId, langCode));
                    _unitOfWork.Commit();
                }

                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }



        public async Task<ResultModel<FileReturnViewModel>> ExportWorkExperience(int userId, int langCode, int type)
        {
            Dictionary<string, string> excelColumnHeader = GetExcelColumnHeader();
            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Relevant_Work_Experiences");
            var result = new ResultModel<FileReturnViewModel>();
            try
            {
                for (int i = 0; i < excelColumnHeader.Count; i++)
                {
                    var columnItem = excelColumnHeader.ElementAt(i);
                    string cellAddress = Convert.ToString(columnItem.Key);
                    string cellValue = Convert.ToString(columnItem.Value);
                    ws.Cell(cellAddress).Value = cellValue;
                    ws.Cell(cellAddress).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                var listWorkkEx = await _workExperienceRepository.GetMulti(x => x.UserId == userId && x.ClassifyWorkingExperience == type, new string[] { "WorkExperienceTrans" });

                int dataRowCounterStart = 3;
                for (int i = 0; i < listWorkkEx.Count; i++)
                {
                    ws.Cell(dataRowCounterStart, 1).Style.DateFormat.Format = "MM/YYYY";
                    ws.Cell(dataRowCounterStart, 1).Value = listWorkkEx[i].FromYear.Month + "-" + listWorkkEx[i].FromYear.Year;
                    ws.Cell(dataRowCounterStart, 2).Style.DateFormat.Format = "MM/YYYY";
                    ws.Cell(dataRowCounterStart, 2).Value = listWorkkEx[i].ToYear.Month + "-" + listWorkkEx[i].ToYear.Year;
                    ws.Cell(dataRowCounterStart, 3).Value = listWorkkEx[i].WorkExperienceTrans.FirstOrDefault(x => x.LanguageId == langCode)?.Location;
                    ws.Cell(dataRowCounterStart, 4).Value = listWorkkEx[i].WorkExperienceTrans.FirstOrDefault(x => x.LanguageId == langCode)?.Position;
                    ws.Cell(dataRowCounterStart, 5).Value = listWorkkEx[i].BIMRelated;
                    ws.Cell(dataRowCounterStart, 6).Value = listWorkkEx[i].WorkExperienceTrans.FirstOrDefault(x => x.LanguageId == langCode)?.JobNature;

                    dataRowCounterStart += 1;
                }

                int lastRowIndex = dataRowCounterStart - 1;
                ws.Columns().AdjustToContents();
                ws.Rows().AdjustToContents();
                ws.Range("A2:F" + lastRowIndex).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A2:F" + lastRowIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                MemoryStream ms = new MemoryStream();
                wb.SaveAs(ms);
                ms.Position = 0;
                result.Message = "Success";
                result.IsSuccess = true;
                result.Data = new FileReturnViewModel()
                {
                    Stream = ms,
                    FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    FileName = "Work_Experience_Export.xlsx"
                };
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw;
            }
        }

        private Dictionary<string, string> GetExcelColumnHeader()
        {
            Dictionary<string, string> excelColumnHeader = new Dictionary<string, string>();
            excelColumnHeader.Add("A2", "Year(From)");
            excelColumnHeader.Add("B2", "Year(To)");
            excelColumnHeader.Add("C2", "Location of Work or Name of Employer");
            excelColumnHeader.Add("D2", "Position and Job Nature");
            excelColumnHeader.Add("E2", "BIM Related(Yes/No)");
            excelColumnHeader.Add("F2", "JobNature");

            return excelColumnHeader;
        }
    }
}
