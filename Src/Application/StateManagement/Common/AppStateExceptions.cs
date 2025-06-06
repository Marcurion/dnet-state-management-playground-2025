namespace Application.StateManagement.Common;

public class HashOutdatedException : Exception;

public class MutexBusyException : Exception;

// Internal latest state should not be filled by the user but by another pipeline behaviour
public class InternalStatePrepopulatedException : Exception;

// Internal latest state should be populated by the pipeline at this point, please check the order pipeline behaviours are registered
public class InternalStateNullException : Exception;

public class HashNotProvidedException : Exception;

public class TooManyReattemptsException : Exception;