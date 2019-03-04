using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyPhotoBL.Enums;
using MyPhotoBL.Models;
using MyPhotoDb.Enums;

namespace MyPhotoBL
{
    public class PictureFilter
    {   
        public ICollection<PictureDetailModel> SourcePictureCollection { get; set; }

        public PictureOrderByField OrderByField { get; set; } = PictureOrderByField.Name;
        
        public bool SortOrderDesc { get; set; } = false;

        public DateTime? FromDate { get; set; } = null;

        public DateTime? ToDate { get; set; } = null;

        public IntegerFilter ResolutionHeightFilter { get; set; } = null;

        public IntegerFilter ResolutionWidthFilter { get; set; } = null;

        public EntityImageFormat FormatFilter { get; set; } = EntityImageFormat.None;

        public string TextFilter { get; set; } = string.Empty;

        public PictureFilter(ICollection<PictureDetailModel> sourceCollection)
        {
            SourcePictureCollection = sourceCollection;
        }

        public ICollection<PictureListModel> GetFilteretPictureListModels()
        {
            var result = SourcePictureCollection.AsEnumerable();
            result = FilterFormat(result);
            result = FilterByText(result);
            result = FilterByHeight(result);
            result = FilterByWidth(result);
            result = FilterFromDate(result);
            result = FilterToDate(result);
            result = OrderPictures(result);
            return result.Select(detail => new PictureListModel {Id = detail.Id, Name = detail.Name, Source = detail.Source}).ToList();
        }

        private IEnumerable<PictureDetailModel> FilterByHeight(IEnumerable<PictureDetailModel> pictures) => 
            ResolutionHeightFilter != null ? pictures.Where(p => ResolutionHeightFilter.Filter(p.ResolutionHeight)) : pictures;

        private IEnumerable<PictureDetailModel> FilterByWidth(IEnumerable<PictureDetailModel> pictures) =>
            ResolutionWidthFilter != null ? pictures.Where(p => ResolutionWidthFilter.Filter(p.ResolutionWidth)) : pictures;

        protected IEnumerable<PictureDetailModel> FilterFormat(IEnumerable<PictureDetailModel> pictures) =>
            FormatFilter != EntityImageFormat.None ? pictures.Where(p => p.Format == FormatFilter) : pictures;

        private IEnumerable<PictureDetailModel> FilterByText(IEnumerable<PictureDetailModel> pictures) => 
            TextFilter != null && !TextFilter.Equals(string.Empty) ? pictures.Where(p => (p.Name + p.Description).ToLower().Contains(TextFilter.ToLower())) : pictures;

        protected IEnumerable<PictureDetailModel> FilterToDate(IEnumerable<PictureDetailModel> pictures) => 
            ToDate != null ? pictures.Where(p => p.PhotoTakenDate <= ToDate) : pictures;

        protected IEnumerable<PictureDetailModel> FilterFromDate(IEnumerable<PictureDetailModel> pictures) =>
            FromDate != null ? pictures.Where(p => p.PhotoTakenDate >= FromDate) : pictures;

        protected IEnumerable<PictureDetailModel> OrderPictures(IEnumerable<PictureDetailModel> pictures)
        {
            if (SortOrderDesc)
            {
                switch (OrderByField)
                {
                    case PictureOrderByField.Date: return pictures.OrderByDescending(p => p.PhotoTakenDate);
                    case PictureOrderByField.Name: return pictures.OrderByDescending(p => p.Name);
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            switch (OrderByField)
            {
                case PictureOrderByField.Date: return pictures.OrderBy(p => p.PhotoTakenDate);
                case PictureOrderByField.Name: return pictures.OrderBy(p => p.Name);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetWidthResolutionFilter(ref string filterString)
        {
            if (filterString == string.Empty)
                return NullWidthFilter();

            if (!int.TryParse(filterString, out var val))
            {
                filterString = string.Empty;
                return NullWidthFilter();
            }

            if (ResolutionWidthFilter == null)
            {
                ResolutionWidthFilter = new IntegerFilter() {FilterValue = val};
                return true;
            }

            if (ResolutionWidthFilter.FilterValue == val)
                return false;

            ResolutionWidthFilter.FilterValue = val;
            return true;
        }



        public bool SetHeightResolutionFilter(ref string filterString)
        {
            if (filterString == string.Empty)
                return NullHeightFilter();

            if (!int.TryParse(filterString, out var val))
            {
                filterString = string.Empty;
                return NullHeightFilter();
            }

            if (ResolutionHeightFilter == null)
            {
                ResolutionHeightFilter = new IntegerFilter() { FilterValue = val };
                return true;
            }

            if (ResolutionHeightFilter.FilterValue == val)
                return false;

            ResolutionHeightFilter.FilterValue = val;
            return true;
        }

        private bool NullWidthFilter()
        {
            if (ResolutionWidthFilter == null)
                return false;
            ResolutionWidthFilter = null;
            return true;
        }

        private bool NullHeightFilter()
        {
            if (ResolutionHeightFilter == null)
                return false;
            ResolutionHeightFilter = null;
            return true;
        }
    }
}
