add_executable(day7)
target_sources(day7
    PRIVATE
    day7.cpp)
target_link_libraries(day7 read_file)
target_include_directories(day7 PRIVATE ../utils)
