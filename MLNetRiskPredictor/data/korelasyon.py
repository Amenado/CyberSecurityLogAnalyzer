import pandas as pd

file_path = "C:/Users/BoraErki/Desktop/CyberSecurityLogAnalyzer/MLNetRiskPredictor/data/benign_ddos.csv"

try:
    df = pd.read_csv(file_path)
    df['Label'] = df['Label'].astype(int) 
except FileNotFoundError:
    print(f"Hata: {file_path} dosyası bulunamadı. Lütfen dosya yolunu kontrol edin.")
    exit()

features = df.drop('Label', axis=1)

correlations = features.corrwith(df['Label']).abs().sort_values(ascending=False)

print("Label sütunu ile en yüksek korelasyona sahip özellikler:")
print("-" * 50)
print(correlations)