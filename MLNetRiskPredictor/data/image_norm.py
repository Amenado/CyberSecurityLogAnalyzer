import pandas as pd

# CSV'yi oku
df = pd.read_csv("C:/Users/BoraErki/Desktop/CyberSecurityLogAnalyzer/MLNetRiskPredictor/data/Friday-WorkingHours-Afternoon-DDos.pcap_ISCX.csv")

# Label sütununu dönüştür
df[" Label"] = df[" Label"].map({"BENIGN": 0, "DDoS": 1})

# Kontrol et
print(df[" Label"].value_counts())

# Yeni CSV olarak kaydet
df.to_csv("benign_ddos.csv", index=False)
