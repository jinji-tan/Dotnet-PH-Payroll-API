const API_BASE = "http://localhost:5000/api";

// --- Navigation ---
function showSection(id) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active-section'));
    document.getElementById(id + '-sec').classList.add('active-section');
    loadDashboard();
}

// --- Dashboard & Loading ---
async function loadDashboard() {
    try {
        const stats = await fetch(`${API_BASE}/Dashboard`).then(r => r.json());
        document.getElementById('totalEmployees').innerText = stats.totalEmployees;
        document.getElementById('totalPayroll').innerText = `₱${stats.totalPayrollMonthly.toLocaleString()}`;

        const employees = await fetch(`${API_BASE}/Employee`).then(r => r.json());
        document.getElementById('employeeBody').innerHTML = employees.map(emp => `
            <tr>
                <td>${emp.id}</td>
                <td>${emp.fullName}</td>
                <td>₱${emp.dailyRate.toLocaleString()}</td>
                <td>
                    <button class="btn btn-pay" onclick="generatePay(${emp.id})">💰 Pay</button>
                    <button class="btn btn-view" onclick="viewAttendance(${emp.id})">👁️ View</button>
                    <button class="btn btn-edit" onclick="updateRate(${emp.id}, '${emp.fullName}')">✏️</button>
                    <button class="btn btn-delete" onclick="deleteEmployee(${emp.id})">🗑️</button>
                </td>
            </tr>`).join('');

        document.getElementById('attEmployeeId').innerHTML = '<option value="">Select...</option>' +
            employees.map(e => `<option value="${e.id}">${e.fullName}</option>`).join('');
    } catch (err) { 
        console.error("Sync failed", err); 
    }
}

// --- Employee Management ---
function filterEmployees() {
    const term = document.getElementById('employeeSearch').value.toLowerCase();
    document.querySelectorAll('#employeeBody tr').forEach(row => {
        row.style.display = row.cells[1].innerText.toLowerCase().includes(term) ? "" : "none";
    });
}

document.getElementById('employeeForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = {
        firstName: document.getElementById('fName').value,
        lastName: document.getElementById('lName').value,
        middleName: document.getElementById('mName').value,
        dateOfBirth: document.getElementById('dob').value,
        dailyRate: parseFloat(document.getElementById('rate').value),
        workingDays: document.getElementById('schedule').value
    };
    await fetch(`${API_BASE}/Employee`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    alert("Saved!"); 
    loadDashboard(); 
    e.target.reset();
});

// --- Attendance ---
document.getElementById('attendanceForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = {
        employeeId: parseInt(document.getElementById('attEmployeeId').value),
        recordDate: document.getElementById('attDate').value,
        isPresent: document.getElementById('attStatus').value === "true"
    };
    const res = await fetch(`${API_BASE}/Attendance`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (res.ok) alert("Logged!"); 
    e.target.reset();
});

// --- Payroll & Actions ---
async function generatePay(id) {
    const start = prompt("Start Date:", "2026-02-23");
    const end = prompt("End Date:", "2026-02-28");
    if (!start || !end) return;

    try {
        const res = await fetch(`${API_BASE}/Payslips/generate/${id}?startDate=${start}&endDate=${end}`, {
            method: 'POST',
            headers: { 'Accept': 'application/json' }
        });

        if (!res.ok) { alert("⚠️ " + await res.text()); return; }

        const result = await res.json();
        const gross = result.grossPay ?? 0;
        const sss = result.ssS_Deduction ?? 0;
        const philHealth = result.philHealth_Deduction ?? 0;
        const pagIbig = result.pagIBIG_Deduction ?? 0;
        const tax = result.tax_Withheld ?? 0;
        const net = result.netPay ?? 0;
        const totalDeductions = sss + philHealth + pagIbig + tax;

        alert(`✅ PAYSLIP GENERATED\n` +
            `----------------------------\n` +
            `Gross Pay: ₱${gross.toLocaleString()}\n` +
            `----------------------------\n` +
            `SSS: ₱${sss.toLocaleString()}\n` +
            `PhilHealth: ₱${philHealth.toLocaleString()}\n` +
            `Pag-IBIG: ₱${pagIbig.toLocaleString()}\n` +
            `Withholding Tax: ₱${tax.toLocaleString()}\n` +
            `----------------------------\n` +
            `Total Deductions: ₱${totalDeductions.toLocaleString()}\n` +
            `----------------------------\n` +
            `NET PAY: ₱${net.toLocaleString()}`);

        loadDashboard();
    } catch (err) {
        alert("❌ Error displaying results.");
    }
}

async function viewAttendance(id) {
    const res = await fetch(`${API_BASE}/Attendance/${id}?startDate=2026-02-01&endDate=2026-02-28`);
    const records = await res.json();
    alert(records.map(r => `${r.recordDate.split('T')[0]}: ${r.isPresent ? '✅' : '❌'}`).join('\n') || "No logs.");
}

async function deleteEmployee(id) {
    if (confirm("Delete?")) { 
        await fetch(`${API_BASE}/Employee/${id}`, { method: 'DELETE' }); 
        loadDashboard(); 
    }
}

async function updateRate(id, fullName) {
    const rate = prompt("New rate:");
    if (!rate) return;
    const names = fullName.split(' ');
    await fetch(`${API_BASE}/Employee/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 
            firstName: names[0], 
            lastName: names[names.length - 1], 
            dailyRate: parseFloat(rate), 
            dateOfBirth: "1995-01-01", 
            workingDays: "MWF" 
        })
    });
    loadDashboard();
}

// Initialize
window.onload = loadDashboard;