import pandas as pd
from python_functions.duplicate_functions import drop_dup_ext2, drop_dup_ext1
from python_functions.anomaly_functions import repeat_anomaly, duplicate_anomaly
from sys import argv
import json

date_from = '2022-04-15'
date_to = '2022-04-20'
path_to_db = ''
json_file='anomaly.json'
anomaly_type = 1

# подготовка словаря JSON
anomaly_dict = {'repeat_report': {'visualisation':None, 'anomaly_anount':0},
                 'duplicate': {'visualisation':None, 'anomaly_anount':0}}

# задание периода выборки, типа аномалии и расположения данных                 
null, path_to_db, date_from, date_to, anomaly_type = argv

ext2 = pd.read_csv(path_to_db + '\\db2\\Ext2.csv')
ext1 = pd.read_csv(path_to_db + '\\db2\\Ext.csv')
catch = pd.read_csv(path_to_db + '\\db1\\catch.csv') 
product = pd.read_csv(path_to_db + '\\db1\\product.csv') 

# первичная обработка данных
ext1.rename(columns={'id_fishery':'id_ves'}, inplace=True)
catch.date = pd.to_datetime(catch.date)
product.date = pd.to_datetime(product.date)
ext1.date_fishery = pd.to_datetime(ext1.date_fishery) 
ext2.date_vsd = pd.to_datetime(ext2.date_vsd) 

# выборка по датам
samp_1 = ext1[(ext1.date_fishery >= pd.to_datetime(date_from)) & (ext1.date_fishery <= pd.to_datetime(date_to))]
samp_2 = ext2[(ext2.date_vsd >= pd.to_datetime(date_from)) & (ext2.date_vsd <= pd.to_datetime(date_to))]


if anomaly_type == 1:
    # аномалия "дубликаты по ключу"
    # нужны данные, подобные ext1
    duplicate, anomaly_amount = duplicate_anomaly(samp_1, samp_2)
    anomaly_dict['duplicate']['visualisation'] = duplicate
    anomaly_dict['duplicate']['anomaly_amount'] = anomaly_amount


# убрать дубликаты в ext2 и подобных файлах
samp_2 = drop_dup_ext2(samp_2)
# убрать дубликаты в ext1 и подобных файлах
samp_1 = drop_dup_ext1(samp_1)


if anomaly_type == 2:
    # аномалия "повторный отчет"
    # нужны данные, подобные ext2
    repeat_report, anomaly_amount = repeat_anomaly(samp_2)
    anomaly_dict['repeat_report']['visualisation'] = repeat_report
    anomaly_dict['repeat_report']['anomaly_amount'] = anomaly_amount



# Машиночитаемый вид
with open(json_file, 'w', encoding='utf8') as f:
    json.dump(anomaly_dict, f)
