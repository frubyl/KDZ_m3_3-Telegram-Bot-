using AdditionalLibrary;
namespace DataProcessing
{
    public static class MethodsForWorkWithData
    {
        /// <summary>
        /// Сортировка.
        /// </summary>
        /// <param name="list"> Объекты для сортировки. </param>
        /// <param name="AscendingOrDescending"> true - сортировка по возрастанию, false - по убыванию. </param>
        /// <returns> Отсортированный лист. </returns>
        public static List<GasStation> Sort(List<GasStation> list, bool AscendingOrDescending)
        {
            list = (from p in list
                    orderby p.TestDate
                    select p).ToList();
            if (!AscendingOrDescending)
            {
                list.Reverse();
            }
            return list;
        }
        /// <summary>
        /// Фильтр.
        /// </summary>
        /// <param name="list"> Объекты для фильтрации. </param>
        /// <param name="dataForFilter"> Данные, по которым происходит фильтрация. </param>
        /// <param name="dataForFilter2"> Данные, по которым происходит фильтрация для поля Owner пункта AdmAreaOwner. </param>
        /// <param name="fieldName"> Имя поля для сортровки. </param>
        /// <returns> Отсортированный лист. </returns>
        public static List<GasStation> Filter(List<GasStation> list, string dataForFilter, string dataForFilter2, string fieldName)
        {
            switch (fieldName)
            {
                case "Owner":
                    list = (list.Where(x => x.Owner.Contains(dataForFilter.Trim()))).ToList();
                    break;
                case "District":
                    list = (list.Where(x => x.District.Contains(dataForFilter.Trim()))).ToList();
                    break;
                case "AdmAreaOwner":
                    list = (list.Where(x => x.AdmArea.Contains(dataForFilter.Trim()) & x.Owner.Contains(dataForFilter2.Trim()))).ToList();
                    break;
            }
            return list;
        }

    }
}
