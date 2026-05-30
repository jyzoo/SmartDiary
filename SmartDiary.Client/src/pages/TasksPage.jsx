import { useNavigate } from "react-router-dom";

export default function TasksPage() {

    const navigate = useNavigate();

    const username =
        localStorage.getItem("username");

    const logout = () => {

        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        localStorage.removeItem("username");

        navigate("/login");
    };

    return (
        <div>

            <nav
                style={{
                    display: "flex",
                    justifyContent: "space-between",
                    padding: "15px",
                    borderBottom: "1px solid gray"
                }}
            >
                <div>
                    Пользователь: {username}
                </div>

                <button onClick={logout}>
                    Выйти
                </button>
            </nav>

            <div style={{ padding: "20px" }}>
                <h1>Tasks</h1>
            </div>

        </div>
    );
}