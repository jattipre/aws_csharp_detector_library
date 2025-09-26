class ProfilePage {
    // BAD: No authorization is used
    protected void btn1_Edit_Click(object sender, EventArgs e) {
        
    }

    // GOOD: `User.IsInRole` checks the current user's role.
    protected void btn2_Delete_Click(object sender, EventArgs e) {
        if (!User.IsInRole("admin")) {
            return;
        }
        
    }
}

internal class User
{
    internal static bool IsInRole(string v)
    {
        throw new NotImplementedException();
    }
}