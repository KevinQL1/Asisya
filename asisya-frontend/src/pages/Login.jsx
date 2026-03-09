import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';

const Login = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [authError, setAuthError] = useState('');
    const navigate = useNavigate();

    const onSubmit = async (data) => {
        try {
            setAuthError('');
            const response = await api.post('/Auth/login', {
                username: data.username,
                password: data.password
            });

            if (response.data && response.data.token) {
                localStorage.setItem('token', response.data.token);
                navigate('/productos');
            }
        } catch (error) {
            setAuthError('Usuario o contraseña incorrectos.');
        }
    };

    return (
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            height: '100vh',
            width: '100vw',
            backgroundColor: '#f5f5f5',
            position: 'fixed',
            top: 0,
            left: 0
        }}>
            <div style={{
                width: '100%',
                maxWidth: '400px',
                padding: '40px',
                backgroundColor: 'white',
                border: '1px solid #ddd',
                borderRadius: '12px',
                boxShadow: '0 4px 20px rgba(0,0,0,0.1)',
                fontFamily: 'sans-serif'
            }}>
                <h2 style={{ textAlign: 'center', marginBottom: '30px', color: '#333' }}>Iniciar Sesión</h2>

                <form onSubmit={handleSubmit(onSubmit)}>
                    <div style={{ marginBottom: '20px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Usuario:</label>
                        <input
                            type="text"
                            {...register("username", { required: "El usuario es obligatorio" })}
                            style={{
                                width: '100%',
                                padding: '12px',
                                boxSizing: 'border-box',
                                borderRadius: '6px',
                                border: '1px solid #ccc'
                            }}
                        />
                        {errors.username && <span style={{ color: 'red', fontSize: '12px' }}>{errors.username.message}</span>}
                    </div>

                    <div style={{ marginBottom: '20px' }}>
                        <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>Contraseña:</label>
                        <input
                            type="password"
                            {...register("password", { required: "La contraseña es obligatoria" })}
                            style={{
                                width: '100%',
                                padding: '12px',
                                boxSizing: 'border-box',
                                borderRadius: '6px',
                                border: '1px solid #ccc'
                            }}
                        />
                        {errors.password && <span style={{ color: 'red', fontSize: '12px' }}>{errors.password.message}</span>}
                    </div>

                    {authError && <div style={{ color: 'red', marginBottom: '20px', fontSize: '14px', textAlign: 'center' }}>{authError}</div>}

                    <button type="submit" style={{
                        width: '100%',
                        padding: '14px',
                        backgroundColor: '#007bff',
                        color: 'white',
                        border: 'none',
                        borderRadius: '8px',
                        cursor: 'pointer',
                        fontSize: '16px',
                        fontWeight: 'bold',
                        transition: 'background-color 0.2s'
                    }}>
                        Ingresar
                    </button>
                </form>
            </div>
        </div>
    );
};

export default Login;