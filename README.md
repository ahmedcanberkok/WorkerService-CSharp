
# WorkerService-CSharp

Bu proje, belirli saatlerde bir dosyaya mesaj yazmak için bir Windows Hizmeti olarak çalışan bir .NET Worker Service uygulamasıdır.

## Proje Açıklaması

WorkerService-CSharp, belirli saatlerde belirtilen bir klasörde günlük dosyaları oluşturur ve bu dosyalara saat bilgisi ile birlikte bir mesaj yazar. Her gün için yeni bir dosya oluşturulur ve o gün içinde belirtilen saatlerde eşleşen saatler bu dosyaya yazılır.

## Özellikler

- Belirli saatlerde belirtilen bir klasörde günlük dosyaları oluşturur.
- Her gün için yeni bir dosya oluşturur.
- Eşleşen saatlerde dosyaya mesaj yazar.
- Loglama yaparak hizmetin çalışma durumu hakkında bilgi verir.

## Kurulum

1. Bu projeyi yerel makinenize klonlayın:
    ```sh
    git clone https://github.com/ahmedcanberkok/WorkerService-CSharp.git
    ```

2. Proje dizinine gidin:
    ```sh
    cd WorkerService-CSharp
    ```

3. Gerekli NuGet paketlerini yükleyin:
    ```sh
    dotnet restore
    ```

4. Projeyi derleyin:
    ```sh
    dotnet build
    ```

## Yayınlama ve Kurulum

1. Projeyi yayınlayın:
    ```sh
    dotnet publish -c Release -r win-x64 -o C:\WorkerServicePublish
    ```

2. Windows Hizmeti olarak kaydedin:
    ```sh
    sc create WorkerService binPath= "C:\WorkerServicePublish\WorkerService1.exe"
    ```

3. Hizmeti başlatın:
    ```sh
    sc start WorkerService
    ```

## Kullanım

- `times.txt` dosyasında belirtilen saatlere göre hizmet çalışacak ve saatler eşleştiğinde `D:\saat-text` klasörüne günlük dosyalar oluşturacaktır.
- 'times.txt' dosyasını sabit bir yerde oluşturup içeriye saat belirttiğinizde   service dosyayı okuyup işlevini yerine getirecektir.
- Günlük dosyaların oluşacağı konumu  değiştirebilirsiniz.

## `Worker.cs` Açıklaması

- `Worker` sınıfı, arka planda çalışan bir servistir.
- `ExecuteAsync` metodu, hizmetin ana işlevlerini yerine getirir. Belirtilen saatleri kontrol eder ve eşleşen saatlerde mesaj yazar.
- `ReadTimesFromFileAsync` metodu, `times.txt` dosyasındaki saatleri okur.
- `AppendTextToFileAsync` metodu, eşleşen saatlerde günlük dosyasına mesaj yazar.

### `Program.cs`

- `Program.cs` dosyası, uygulamanın giriş noktasıdır ve `UseWindowsService` yöntemi ile uygulamanın bir Windows hizmeti olarak çalışmasını sağlar.
