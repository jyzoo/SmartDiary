import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import api from "../services/api";
import { useNavigate } from "react-router-dom";

const validationSchema = Yup.object({
    username: Yup.string()
        .required("Введите логин"),

    password: Yup.string()
        .required("Введите пароль")
});

export default function LoginPage() {

    const navigate = useNavigate();

    return (
        <div style={{ padding: "20px" }}>
            <h1>Вход</h1>

            <Formik
                initialValues={{
                    username: "",
                    password: ""
                }}
                validationSchema={validationSchema}
                onSubmit={async (values) => {

    try {

        const response =
            await api.post(
                "/auth/login",
                values
            );

        localStorage.setItem(
            "accessToken",
            response.data.accessToken
        );

        localStorage.setItem(
            "refreshToken",
            response.data.refreshToken
        );

        localStorage.setItem(
    "username",
    response.data.username
);



        alert("Успешный вход");

        navigate("/tasks");

    } catch (error) {

    console.log(error);

    alert("Ошибка входа");

}
}}
            >
                <Form>

                    <div>
                        <label>Логин</label>
                        <br />

                        <Field
                            name="username"
                            type="text"
                        />

                        <ErrorMessage
                            name="username"
                            component="div"
                        />
                    </div>

                    <br />

                    <div>
                        <label>Пароль</label>
                        <br />

                        <Field
                            name="password"
                            type="password"
                        />

                        <ErrorMessage
                            name="password"
                            component="div"
                        />
                    </div>

                    <br />

                    <button type="submit">
                        Войти
                    </button>

                </Form>
            </Formik>
        </div>
    );
}