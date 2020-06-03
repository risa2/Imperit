namespace Imperit.Services
{
    public interface IPassword
    {
        void Set(string new_pw);
        bool IsCorrect(string pw);
    }
    public class Password : IPassword
    {
        private Load.IFile input;
        public Password(IServiceIO io) => this.input = io.Password;
        public bool IsCorrect(string pw) => State.Password.FromString(pw) == State.Password.Parse(input.Read());
        public void Set(string new_pw) => input.Write(State.Password.FromString(new_pw).ToString());
    }
}