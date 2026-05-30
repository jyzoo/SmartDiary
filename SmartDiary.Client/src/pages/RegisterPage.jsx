import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import api from "../services/api";

const validationSchema = Yup.object({
    username: Yup.string()
        .required("Введите логин"),

    email: Yup.string()
        .email("Неверный email")
        .required("Введите email"),

    password: Yup.string()
        .min(6, "Минимум 6 символов")
        .required("Введите пароль")
});

export default function RegisterPage() {
    return (
        <div style={{ padding: "20px" }}>
            <h1>Регистрация</h1>

            <Formik
                initialValues={{
                    username: "",
                    email: "",
                    password: ""
                }}
                validationSchema={validationSchema}
                onSubmit={async (values) => {

    try {

        await api.post(
            "/auth/register",
            values
        );

        alert("Пользователь создан");

    } catch {

        alert("Ошибка регистрации");

    }
}}
            >
                <Form>

                    <div>
                        <label>Логин</label>
                        <br />

                        <Field name="username" />

                        <ErrorMessage
                            name="username"
                            component="div"
                        />
                    </div>

                    <br />

                    <div>
                        <label>Email</label>
                        <br />

                        <Field name="email" />

                        <ErrorMessage
                            name="email"
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
                        Зарегистрироваться
                    </button>

                </Form>
            </Formik>
        </div>
    );
}