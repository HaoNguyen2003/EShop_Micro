# Tài liệu Docker Compose - EShop Micro

## Tổng quan

Dự án dùng **hai file** Docker Compose:

| File | Vai trò |
|------|--------|
| `docker-compose.yml` | File cơ sở: định nghĩa **services** và **volumes** (dùng chung cho mọi môi trường). |
| `docker-compose.override.yml` | File ghi đè: thêm cấu hình **môi trường cụ thể** (biến môi trường, port, volume mount, restart). |

Khi chạy `docker-compose up`, Docker Compose **tự động merge** hai file: cấu hình trong `docker-compose.override.yml` sẽ **ghi đè/bổ sung** vào `docker-compose.yml`.

---

## 1. docker-compose.yml (file cơ sở)

```yaml
services:
  catalogdb:
   image: postgres

volumes:
  postgres_catalog:
```

### Giải thích

- **`services.catalogdb`**: Định nghĩa một service tên `catalogdb`.
  - **`image: postgres`**: Dùng image Docker chính thức [postgres](https://hub.docker.com/_/postgres) (PostgreSQL).

- **`volumes.postgres_catalog`**: Định nghĩa volume có tên `postgres_catalog`.
  - Volume này dùng để **lưu dữ liệu PostgreSQL** (database, bảng, ...) ngoài container, tránh mất dữ liệu khi xóa/recrate container.

File này **không** khai báo port, biến môi trường hay mount volume; những thứ đó nằm trong file override.

---

## 2. docker-compose.override.yml (file ghi đè)

```yaml
services:
  catalogdb:
    container_name: catalogdb
    environment:
      - POSTGRES_USER = postgres
      - POSTGRES_PASSWORD = postgres
      - POSTGRES_DB = catalogDb
    restart: always
    ports:
      - "5432:5432"
    volumes:
      - postgres_catalog:/var/lib/postgresql/data
```

### Giải thích từng mục

| Mục | Ý nghĩa |
|-----|--------|
| **`container_name: catalogdb`** | Tên container khi chạy là `catalogdb` (dễ nhận diện khi `docker ps`). |
| **`environment`** | Biến môi trường cho PostgreSQL: user, mật khẩu và tên database. |
| **`restart: always`** | Container tự khởi động lại khi bị dừng hoặc khi Docker Desktop khởi động lại. |
| **`ports: "5432:5432"`** | Map port **5432** trên máy host → port **5432** trong container (port mặc định của PostgreSQL). Ứng dụng trên máy bạn kết nối qua `localhost:5432`. |
| **`volumes: postgres_catalog:/var/lib/postgresql/data`** | Mount volume `postgres_catalog` (định nghĩa trong file cơ sở) vào thư mục dữ liệu của PostgreSQL trong container. Dữ liệu được lưu trong volume, không mất khi xóa container. |

### Lưu ý quan trọng (environment)

Trong file override hiện tại có **khoảng trắng** quanh dấu `=`:

```yaml
- POSTGRES_USER = postgres   # có thể gây lỗi
```

Nên sửa thành (không có khoảng trắng quanh `=`):

```yaml
- POSTGRES_USER=postgres
- POSTGRES_PASSWORD=postgres
- POSTGRES_DB=catalogDb
```

Một số phiên bản Docker Compose/PostgreSQL có thể coi giá trị là `" postgres"` (có dấu cách) nếu để khoảng trắng.

---

## Luồng khi chạy

1. Chạy: `docker-compose up -d` (hoặc từ Visual Studio: Start Docker Compose).
2. Docker Compose đọc `docker-compose.yml` + `docker-compose.override.yml` và merge.
3. Tạo volume `postgres_catalog` (nếu chưa có).
4. Chạy container `catalogdb` với:
   - Image: `postgres`
   - Port: `5432:5432`
   - Env: user/password/db như trên
   - Dữ liệu lưu trong volume `postgres_catalog`.

### Kết nối từ ứng dụng

- **Host:** `localhost` (hoặc `127.0.0.1`)
- **Port:** `5432`
- **Database:** `catalogDb`
- **User:** `postgres`
- **Password:** `postgres`

Ví dụ connection string:

```
Host=localhost;Port=5432;Database=catalogDb;Username=postgres;Password=postgres
```

---

## Tóm tắt

- **docker-compose.yml**: Chỉ định service `catalogdb` (image postgres) và volume `postgres_catalog`.
- **docker-compose.override.yml**: Thêm tên container, biến môi trường, port, restart và mount volume cho `catalogdb`.
- Chạy `docker-compose up` sẽ dùng cả hai file; dữ liệu PostgreSQL được lưu trong volume nên an toàn khi xóa container.

---

## Khắc phục lỗi DT1001 / Build failed

Khi gặp lỗi **DT1001** với nội dung dạng:
- *"View build details: docker-desktop://dashboard/build/...*"
- *"If the error persists, try restarting Docker Desktop"*

Nghĩa là **build Docker (thường là image `catalog.api`) đã thất bại**. Nguyên nhân hay gặp:

### 1. Chưa bật Volume sharing (ổ đĩa chứa project)

Project nằm trên ổ **D:** (`D:\Document_Microservice_Code\...`). Docker Desktop cần **quyền truy cập** ổ đó để build image.

**Cách xử lý:**

1. Mở **Docker Desktop**.
2. Vào **Settings** (biểu tượng bánh răng).
3. Chọn **Resources** → **File sharing** (hoặc **Shared Drives** tùy phiên bản).
4. Bật / chọn **ổ D:** (hoặc thư mục chứa project, ví dụ `D:\Document_Microservice_Code`).
5. Bấm **Apply & Restart**.

Sau khi Docker khởi động lại, thử chạy lại Docker Compose từ Visual Studio.

### 2. Docker Desktop cần khởi động lại

Nếu đã bật sharing mà vẫn lỗi:

1. **Restart Docker Desktop**: chuột phải icon Docker ở khay hệ thống → **Restart**.
2. Đợi Docker chạy ổn định (icon không còn “starting”).
3. Chạy lại project (Docker Compose).

### 3. Xem chi tiết lỗi build

- Trong thông báo lỗi có link **"View build details"** → mở link (mở bằng Docker Desktop) để xem log build.
- Log sẽ ghi rõ bước nào fail (restore, build, copy file,…) để xử lý tiếp (ví dụ: thiếu file, sai đường dẫn, lỗi mạng).
