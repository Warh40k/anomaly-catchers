import json
import pandas as pd
from tqdm import tqdm
def repeat_anomaly(ext2, txt_file='anomaly.txt', hours_delay=1):
    anomaly_count = 0
    anomaly_dict = {'violation':[], 'mistake':[]}
    tmp = ext2[ext2.volume > 0].sort_values(by='volume')
    for i in tqdm(range(tmp.shape[0] - 1)):
        lines = tmp.iloc[i:i+2]
        c1 = lines.volume.iloc[0] == lines.volume.iloc[1]
        c2 = lines.unit.iloc[0] != lines.unit.iloc[1]
        c3 = pd.Timedelta(lines.date_vsd.iloc[0] - lines.date_vsd.iloc[1]).seconds / 3600.0 < hours_delay
        c4 = lines.fish.iloc[0] == lines.fish.iloc[1]
        lines.date_vsd = lines.date_vsd.astype(str)
        if c1 & c2 & c3 & c4:
            anomaly_dict['mistake'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json()) 
            anomaly_count += 1         
        if (c1 & c2 & c4) and not c3:
            anomaly_dict['violation'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json())          
            anomaly_count += 1         

    print(txt_file)
    # Человекочитаемый вид
    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях repeat_report\n')
        print(f'Выявлено {anomaly_count} аномалий\n', file=f)
        print(f'Задержки в пределах допустимого:\n', file=f)
        for line in anomaly_dict['mistake']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f) 
        print(file=f)
        print('-' * 40, file=f)
        print(file=f)
        print(f'Задержки больше {hours_delay} часа(ов):\n', file=f)
        for line in anomaly_dict['violation']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f)

    return anomaly_dict, anomaly_count
    
def duplicate_anomaly(ext1, ext2, txt_file = 'anomaly.txt'):

    # df2 = ext2.copy()
    # initial_drop = df2[df2.duplicated(['id_vsd', 'fish'], keep=False)].index
    # df2 = df2.drop(initial_drop)
    # tmp = df2[df2.duplicated('id_vsd', keep=False)]
    # drop_idx = tmp.sort_values(by=['id_vsd', 'unit']).index[1::2]
    # tmp = tmp.drop(drop_idx)

    df1 = ext1.copy()
    tmp1 = df1[df1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves'])
    change_idx = df1[df1.duplicated(['id_vsd'], keep=False)].sort_values(by=['id_vsd', 'id_ves']).index
    df1.loc[change_idx, 'Product_period'] = (tmp1.Product_period + ' - ' + tmp1.date_fishery.astype(str)).values
    df1.loc[change_idx[::2], 'id_ves'] = (df1.loc[change_idx[1::2], 'id_ves']).values
    drop_idx = df1[df1.duplicated(['id_vsd'], keep='first')].sort_values(by=['id_vsd', 'id_ves']).index
    df1 = df1.loc[drop_idx]

    # Человекочитаемый вид
    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях duplicate_anomaly\n', file=f)
        print(f'Выявлено {len(df1)} аномалий (дубликатов)\n', file=f)
        print(f'Владельцам следующих кораблей были отправлены предупреждения:\n', file=f)
        print(df1[['id_own', 'id_ves']].sort_values(by=['id_own', 'id_ves']).value_counts().to_string(), file=f) 


    return df1[['id_ves', 'id_own', 'date_fishery']].to_json(), len(df1)

    duplicate, anomaly_amount
