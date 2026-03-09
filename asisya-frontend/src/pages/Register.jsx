import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';

const Register = () => {
    const [userData, setUserData] = useState({
        username: '',
        password: '',
        firstName: '',
        lastName: '',
        title: '',
        titleOfCourtesy: '',
        birthDate: '',
        hireDate: '',
        address: '',
        city: '',
        region: '',
        postalCode: '',
        country: '',
        homePhone: '',
        extension: '',
        notes: '',
        reportsTo: null,
        photoBase64: ''
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUserData({ ...userData, [name]: value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();

        const cleanedData = { ...userData };

        Object.keys(cleanedData).forEach((key) => {
            if (cleanedData[key] === '' || cleanedData[key] === null) {
                delete cleanedData[key];
            }
        });

        try {
            await api.post('/Auth/register', cleanedData);
            alert("Empleado registrado con éxito.");
            navigate('/login');
        } catch (error) {
            const apiError = error.response?.data?.errors
                ? JSON.stringify(error.response.data.errors)
                : (error.response?.data?.message || "Error desconocido");

            alert("Error al registrar: " + apiError);
            console.error("Detalle del error:", error.response?.data);
        }
    };

    return (
        <div style={{ padding: '110px 20px 40px 20px', maxWidth: '900px', margin: '0 auto', fontFamily: 'sans-serif' }}>
            <form onSubmit={handleRegister} style={{ backgroundColor: 'white', padding: '40px', borderRadius: '12px', boxShadow: '0 4px 20px rgba(0,0,0,0.1)' }}>
                <h2 style={{ textAlign: 'center', marginBottom: '30px', color: '#333' }}>Registro de Nuevo Empleado</h2>

                <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }}>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Usuario (Email):</label>
                        <input required name="username" value={userData.username} placeholder="ejemplo@correo.com" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Contraseña:</label>
                        <input required name="password" type="password" value={userData.password} placeholder="********" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Nombre:</label>
                        <input required name="firstName" value={userData.firstName} placeholder="Nombre" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Apellido:</label>
                        <input required name="lastName" value={userData.lastName} placeholder="Apellido" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Cargo:</label>
                        <input name="title" value={userData.title} placeholder="Ej: Sales Representative" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Tratamiento:</label>
                        <input name="titleOfCourtesy" value={userData.titleOfCourtesy} placeholder="Ej: Mr. / Ms." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Fecha de Nacimiento:</label>
                        <input name="birthDate" type="date" value={userData.birthDate} onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Fecha de Contratación:</label>
                        <input name="hireDate" type="date" value={userData.hireDate} onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Dirección:</label>
                        <input name="address" value={userData.address} placeholder="Calle 123..." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Ciudad:</label>
                        <input name="city" value={userData.city} placeholder="Ciudad" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>País:</label>
                        <input name="country" value={userData.country} placeholder="País" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Teléfono:</label>
                        <input name="homePhone" value={userData.homePhone} placeholder="Teléfono" onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc' }} />
                    </div>

                </div>

                <div style={{ display: 'flex', flexDirection: 'column', marginTop: '20px' }}>
                    <label style={{ fontSize: '14px', marginBottom: '5px', fontWeight: 'bold' }}>Notas adicionales:</label>
                    <textarea name="notes" value={userData.notes} placeholder="Información extra del empleado..." onChange={handleChange} style={{ padding: '12px', borderRadius: '6px', border: '1px solid #ccc', height: '100px', resize: 'vertical' }}></textarea>
                </div>

                <button type="submit" style={{ width: '100%', marginTop: '30px', padding: '15px', backgroundColor: '#28a745', color: 'white', border: 'none', borderRadius: '8px', cursor: 'pointer', fontWeight: 'bold', fontSize: '16px' }}>
                    Finalizar Registro
                </button>
            </form>
        </div>
    );
};

export default Register;