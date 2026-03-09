import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';

const Register = () => {
    const [userData, setUserData] = useState({
        username: '', password: '', firstName: '', lastName: '',
        title: '', titleOfCourtesy: '', birthDate: '', hireDate: '',
        address: '', city: '', region: '', postalCode: '',
        country: '', homePhone: '', extension: '', notes: '',
        reportsTo: null, photoBase64: ''
    });
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUserData({ ...userData, [name]: value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await api.post('/api/Auth/register', userData);
            alert("Empleado registrado con éxito.");
            navigate('/login');
        } catch (error) {
            alert("Error al registrar: " + (error.response?.data?.message || "Verifique los datos"));
        }
    };

    return (
        <div style={{ padding: '110px 20px 40px 20px', maxWidth: '900px', margin: '0 auto', fontFamily: 'sans-serif' }}>
            <form onSubmit={handleRegister} style={{ backgroundColor: 'white', padding: '40px', borderRadius: '12px', boxShadow: '0 4px 20px rgba(0,0,0,0.1)' }}>
                <h2 style={{ textAlign: 'center', marginBottom: '30px', color: '#333' }}>Registro de Nuevo Empleado</h2>
                
                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>
                    
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Usuario (Email):</label>
                        <input required name="username" placeholder="ejemplo@correo.com" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Contraseña:</label>
                        <input required name="password" type="password" placeholder="********" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Nombre:</label>
                        <input required name="firstName" placeholder="Nombre" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Apellido:</label>
                        <input required name="lastName" placeholder="Apellido" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Cargo:</label>
                        <input name="title" placeholder="Ej: Sales Representative" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Tratamiento:</label>
                        <input name="titleOfCourtesy" placeholder="Ej: Mr. / Ms." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Fecha de Nacimiento:</label>
                        <input name="birthDate" type="date" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Fecha de Contratación:</label>
                        <input name="hireDate" type="date" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Dirección:</label>
                        <input name="address" placeholder="Calle 123..." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Ciudad:</label>
                        <input name="city" placeholder="Ciudad" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>País:</label>
                        <input name="country" placeholder="País" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Teléfono:</label>
                        <input name="homePhone" placeholder="Teléfono" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                </div>

                <div style={{ display: 'flex', flexDirection: 'column', marginTop: '20px' }}>
                    <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Notas adicionales:</label>
                    <textarea name="notes" placeholder="Información extra del empleado..." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc', height: '100px', resize: 'vertical' }}></textarea>
                </div>

                <button type="submit" style={{ width: '100%', marginTop: '30px', padding: '15px', backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 'bold', fontSize: '16px' }}>
                    Finalizar Registro
                </button>
            </form>
        </div>
    );
};

export default Register;